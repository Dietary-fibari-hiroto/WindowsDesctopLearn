import 'dart:async';
import 'dart:io';
import 'dart:typed_data';
import 'package:flutter/foundation.dart';
import 'package:flutter/services.dart';


class SpotifyPlayerState{
  final String? trackName;
  final String? artistName;
  final String? albumName;
  final String? trackUri;
  final String? imageUri;
  final int durationMs;
  final int positionMs;
  final bool isPaused;

  const SpotifyPlayerState({
    this.trackName,
    this.artistName,
    this.albumName,
    this.trackUri,
    this.imageUri,
    this.durationMs = 0,
    this.positionMs = 0,
    this.isPaused = true,
  });

  //Androidデータ構造をFlutter型へ変換するヘルパー関数
  factory SpotifyPlayerState.fromMap(Map<dynamic,dynamic> map){
    return SpotifyPlayerState(
      trackName:map['trackName']as String?,
      artistName: map['artistName'] as String?,
      albumName: map['albumName'] as String?,
      trackUri: map['trackUri'] as String?,
      imageUri: map['imageUri'] as String?,
      durationMs: (map['durationMs'] as int?) ?? 0,
      positionMs: (map['positionMs'] as int?) ?? 0,
      isPaused: (map['isPaused'] as bool?) ?? true,
    );
  }

  bool get isPlaying => !isPaused;
  double get progress =>
    durationMs > 0 ? (positionMs/durationMs).clamp(0.0, 1.0) : 0.0;

    bool get hasTrack => trackName != null;

    @override
    String toString()=>
      'SpotifyPlayerState(track=$trackName, artist=$artistName, '
      'position=$positionMs/$durationMs, isPaused=$isPaused)';


}


class SpotifyService extends ChangeNotifier{
  static const _methodChannel = MethodChannel("com.example.native_lib_test/spotify");
  static const _eventChannel = EventChannel("com.example.native_lib_test/spotify_player_state");


  SpotifyPlayerState _playerState = const SpotifyPlayerState();
  bool _isConnected = false;
  String? _error;
  Uint8List? _albumArt;
  String? _currentImageUri;

  StreamSubscription? _playerStateSub;
  Timer? _positionTimer;

  //公開プロパティ(キャメルケース)
  SpotifyPlayerState get playerState => _playerState;
  bool get isConnected => _isConnected;
  String? get error => _error;
  Uint8List? get albumArt => _albumArt;

  Future<void> connect() async{
    try{
      _error = null;
      notifyListeners();

      await _methodChannel.invokeMethod("connect");
      _isConnected = true;
      
      _playerStateSub = _eventChannel
        .receiveBroadcastStream()
        .listen(_onPlayerStateEvent,onError:_onPlayerStateError);

      //再生位置をローカルで補助更新するタイマー
      _startPositionTimer();

      notifyListeners();
      await _fetchInitialState();
    }on PlatformException catch(e){
      _error = e.message ?? '接続に失敗しました。';
      _isConnected = false;
      notifyListeners();
    }
  }

Future<void> _fetchInitialState() async {
    try {
      debugPrint('[SpotifyService] Fetching initial player state...');
      final result = await _methodChannel.invokeMethod('getPlayerState');
      debugPrint('[SpotifyService] Initial state raw: $result');
      if (result is Map) {
        _playerState = SpotifyPlayerState.fromMap(result);
        debugPrint('[SpotifyService] Initial state parsed: $_playerState');
        _fetchAlbumArt(_playerState.imageUri);
        notifyListeners();
      }
    } on PlatformException catch (e) {
      debugPrint(
          '[SpotifyService] Failed to fetch initial state: ${e.message}');
    }
  }

  Future<void> disconnect() async{
    try{
      await _methodChannel.invokeMethod('disconnect');
    }catch(_){}
      _isConnected = false;
      _playerStateSub?.cancel();
      _positionTimer?.cancel();
      _playerState = const SpotifyPlayerState();
      _albumArt = null;
      _currentImageUri = null;
      notifyListeners();
  }

  Future<void> play(String spotifyUri) async{
    await _invoke('play',{'uri':spotifyUri});
  }

  Future<void> resume() async{
    await _invoke('resume');
  }

  Future<void> pause() async{
    await _invoke('pause');
  }

  Future<void> togglePlayPause() async{
    if(_playerState.isPaused){
      await resume();
    }else{
      await pause();
    }
  }

  Future<void> skipNext() async{
    await _invoke('skipNext');
  }
  Future<void> skipPrevious() async{
    await _invoke('skipPrevious');
  }

  Future<void> seekTo(int positionMs) async{
    await _invoke('seekTo',{'positionMs':positionMs});
  }

  Future<void> seekToFraction(double fraction) async{
    final pos = (fraction * _playerState.durationMs).round();
    await seekTo(pos);
  }


  //アルバムアートの取得関数
  Future<void> _fetchAlbumArt(String? imageUri) async{
    if(imageUri == null || imageUri ==_currentImageUri) return;

    try{
      final bytes = await _methodChannel.invokeMethod<Uint8List>('getImage',{
        'imageUri':imageUri
      });
      _albumArt = bytes;
      notifyListeners();
    }catch(e){
      debugPrint('アルバムアートの取得に失敗しました。:$e');
    }
  }

  //PlayerStateを更新するハンドラー
  void _onPlayerStateEvent(dynamic event){
    if(event is Map){
      _playerState = SpotifyPlayerState.fromMap(event);
      _error = null;

      _fetchAlbumArt(_playerState.imageUri);

      notifyListeners();
    }
  }

  void _onPlayerStateError(Object Error){
    _error=error.toString();
    notifyListeners();
  }



  //ローカル再生位置タイマー
  //再生中は 200ms ごとにローカルで positionMs を進める
void _startPositionTimer() {
    _positionTimer?.cancel();
    _positionTimer = Timer.periodic(
      const Duration(milliseconds: 200),
      (_) {
        if (_playerState.isPlaying && _playerState.hasTrack) {
          final newPos = (_playerState.positionMs + 200)
              .clamp(0, _playerState.durationMs);
          _playerState = SpotifyPlayerState(
            trackName: _playerState.trackName,
            artistName: _playerState.artistName,
            albumName: _playerState.albumName,
            trackUri: _playerState.trackUri,
            imageUri: _playerState.imageUri,
            durationMs: _playerState.durationMs,
            positionMs: newPos,
            isPaused: _playerState.isPaused,
          );
          notifyListeners();
        }
      },
    );
  }

  Future<void> _invoke(String method,[Map<String,dynamic>? args]) async{
    try{
      _error = null;
      await _methodChannel.invokeMethod(method,args);
    }on PlatformException catch(e){
      _error = e.message;
      notifyListeners();
    }
  }


  @override
  void dispose(){
    _positionTimer?.cancel();
    _playerStateSub?.cancel();
    spotifyAppRemote_disconnect();
    super.dispose();
  }

  void spotifyAppRemote_disconnect() {
    try {
      _methodChannel.invokeMethod('disconnect');
    } catch (_) {}
  }

}