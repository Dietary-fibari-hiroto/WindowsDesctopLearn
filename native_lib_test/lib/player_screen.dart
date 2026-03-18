import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'spotify_service.dart';

class PlayerScreen extends StatelessWidget{
  const PlayerScreen({super.key});

  @override
  Widget build(BuildContext context){
    return Scaffold(
      appBar:AppBar(
        title:const Text('SpotifyTest'),
        backgroundColor:const  Color(0xFF121212),
        elevation: 0,
      ),
      body: Consumer<SpotifyService>(
        builder: (context,spotify,_) {
          if(!spotify.isConnected){
            return _buildConnectView(context,spotify);
          }

          return _buildPlayerView(context,spotify);
        },
      ),
    );
  }

  Widget _buildPlayerView(BuildContext context, SpotifyService spotify) {
    final state = spotify.playerState;

    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 32),
      child: Column(
        children: [
          const Spacer(flex: 1),

          // ── クイック再生ボタン (テスト用) ──
          _buildQuickPlaySection(spotify),

          const SizedBox(height: 32),

          // ── アルバムアート ──
          _buildAlbumArt(spotify),

          const SizedBox(height: 24),

          // ── 曲情報 ──
          Text(
            state.trackName ?? '再生中の曲はありません',
            style: const TextStyle(
              color: Colors.white,
              fontSize: 22,
              fontWeight: FontWeight.bold,
            ),
            textAlign: TextAlign.center,
            maxLines: 1,
            overflow: TextOverflow.ellipsis,
          ),
          const SizedBox(height: 4),
          Text(
            state.artistName ?? '',
            style: const TextStyle(color: Colors.white60, fontSize: 16),
            textAlign: TextAlign.center,
            maxLines: 1,
            overflow: TextOverflow.ellipsis,
          ),

          const SizedBox(height: 24),

          // ── シークバー ──
          _buildSeekBar(spotify, state),

          const SizedBox(height: 16),

          // ── 再生コントロール ──
          _buildControls(spotify, state),

          const Spacer(flex: 2),

          // ── 切断ボタン ──
          TextButton(
            onPressed: spotify.disconnect,
            child: const Text(
              '切断',
              style: TextStyle(color: Colors.white38),
            ),
          ),
          const SizedBox(height: 16),

          // ── エラー表示 ──
          if (spotify.error != null)
            Padding(
              padding: const EdgeInsets.only(bottom: 16),
              child: Text(
                spotify.error!,
                style: const TextStyle(color: Colors.redAccent, fontSize: 12),
              ),
            ),
        ],
      ),
    );
  }

    Widget _buildQuickPlaySection(SpotifyService spotify) {
    return Row(
      children: [
        Expanded(
          child: ElevatedButton(
            onPressed: () {
              // テスト用にプレイリストを再生
              // 好きな URI に変えてね
              spotify.play('spotify:playlist:37i9dQZF1DXcBWIGoYBM5M');
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.white12,
              foregroundColor: Colors.white,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(8),
              ),
            ),
            child: const Text('Today\'s Top Hits を再生'),
          ),
        ),
      ],
    );
  }

  Widget _buildConnectView(BuildContext context,SpotifyService spotify){
    return Center(
      child:Column(
      mainAxisSize: MainAxisSize.min,
      children: [
        const Icon(Icons.music_note,size:80 ,color:Color(0xFF1DB954)),
        const SizedBox(height: 24,),
        const Text('Spotifyに接続',            style: TextStyle(
              color: Colors.white,
              fontSize: 24,
              fontWeight: FontWeight.bold,
            ),),
        const SizedBox(height: 8),
          const Text(
            'Spotify アプリがインストールされている必要があります',
            style: TextStyle(color: Colors.white54, fontSize: 14),
          ),
        const SizedBox(height: 32),
        ElevatedButton.icon(
          onPressed: spotify.connect,
          icon:const Icon(Icons.link),
          label:const Text('接続する'),
          style: ElevatedButton.styleFrom(
              backgroundColor: const Color(0xFF1DB954),
              foregroundColor: Colors.white,
              padding: const EdgeInsets.symmetric(horizontal: 32, vertical: 16),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(24),
              ),
            ),
        ),
        if (spotify.error != null) ...[
            const SizedBox(height: 16),
            Text(
              spotify.error!,
              style: const TextStyle(color: Colors.redAccent, fontSize: 13),
              textAlign: TextAlign.center,
            ),
          ],
      ],
      )
    );
  }




  Widget _buildAlbumArt(SpotifyService spotify){
    return Container(
      width: 280,
      height: 280,
      decoration: BoxDecoration(
        color: Colors.white10,
        borderRadius: BorderRadius.circular(8),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withOpacity(0.4),
            blurRadius: 24,
            offset: const Offset(0, 8),
          ),
        ],
      ),
      clipBehavior: Clip.antiAlias,
      child: spotify.albumArt != null
          ? Image.memory(spotify.albumArt!, fit: BoxFit.cover)
          : const Center(
              child: Icon(
                Icons.album,
                size: 80,
                color: Colors.white24,
              ),
            ),
    );
  }

  
  Widget _buildSeekBar(SpotifyService spotify, SpotifyPlayerState state) {
    return Column(
      children: [
        SliderTheme(
          data: SliderThemeData(
            trackHeight: 4,
            thumbShape: const RoundSliderThumbShape(enabledThumbRadius: 6),
            activeTrackColor: const Color(0xFF1DB954),
            inactiveTrackColor: Colors.white24,
            thumbColor: Colors.white,
            overlayColor: const Color(0xFF1DB954).withOpacity(0.2),
          ),
          child: Slider(
            value: state.progress,
            onChanged: (value) => spotify.seekToFraction(value),
          ),
        ),
        Padding(
          padding: const EdgeInsets.symmetric(horizontal: 16),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(
                _formatDuration(state.positionMs),
                style: const TextStyle(color: Colors.white54, fontSize: 12),
              ),
              Text(
                _formatDuration(state.durationMs),
                style: const TextStyle(color: Colors.white54, fontSize: 12),
              ),
            ],
          ),
        ),
      ],
    );
  }

  
  Widget _buildControls(SpotifyService spotify, SpotifyPlayerState state) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        IconButton(
          icon: const Icon(Icons.skip_previous, color: Colors.white, size: 36),
          onPressed: spotify.skipPrevious,
        ),
        const SizedBox(width: 16),
        // 再生/停止
        Container(
          decoration: const BoxDecoration(
            color: Colors.white,
            shape: BoxShape.circle,
          ),
          child: IconButton(
            icon: Icon(
              state.isPaused ? Icons.play_arrow : Icons.pause,
              color: Colors.black,
              size: 40,
            ),
            onPressed: spotify.togglePlayPause,
            padding: const EdgeInsets.all(12),
          ),
        ),
        const SizedBox(width: 16),
        IconButton(
          icon: const Icon(Icons.skip_next, color: Colors.white, size: 36),
          onPressed: spotify.skipNext,
        ),
      ],
    );
  }

  String _formatDuration(int ms) {
    final duration = Duration(milliseconds: ms);
    final minutes = duration.inMinutes;
    final seconds = (duration.inSeconds % 60).toString().padLeft(2, '0');
    return '$minutes:$seconds';
  }


}