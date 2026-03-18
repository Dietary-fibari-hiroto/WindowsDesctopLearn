package com.example.native_lib_test

import android.graphics.Bitmap
import android.util.Log
import io.flutter.embedding.android.FlutterActivity
import io.flutter.embedding.engine.FlutterEngine
import io.flutter.plugin.common.EventChannel
import io.flutter.plugin.common.MethodChannel
import com.spotify.android.appremote.api.ConnectionParams
import com.spotify.android.appremote.api.Connector
import com.spotify.android.appremote.api.SpotifyAppRemote
import com.spotify.protocol.types.PlayerState
import com.spotify.protocol.types.ImageUri
import java.io.ByteArrayOutputStream



class MainActivity : FlutterActivity() {

    companion object {
        private const val TAG = "SpotifyBridge"
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // ★ ここを自分の Client ID に書き換える
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        private const val CLIENT_ID = ""
        private const val REDIRECT_URI = ""

        private const val METHOD_CHANNEL = "com.example.native_lib_test/spotify"
        private const val EVENT_CHANNEL = "com.example.native_lib_test/spotify_player_state"
    }

    private var spotifyAppRemote: SpotifyAppRemote? = null
    private var playerStateEventSink: EventChannel.EventSink? = null

    // ──────────────────────────────────────
    // FlutterEngine 設定
    // ──────────────────────────────────────
    override fun configureFlutterEngine(flutterEngine: FlutterEngine) {
        super.configureFlutterEngine(flutterEngine)

        // MethodChannel: Flutter → Native の呼び出し
        MethodChannel(flutterEngine.dartExecutor.binaryMessenger, METHOD_CHANNEL)
            .setMethodCallHandler { call, result ->
                when (call.method) {
                    "connect" -> connect(result)
                    "disconnect" -> disconnect(result)
                    "play" -> {
                        val uri = call.argument<String>("uri")
                        if (uri != null) play(uri, result)
                        else result.error("INVALID_ARG", "uri is required", null)
                    }
                    "resume" -> resume(result)
                    "pause" -> pause(result)
                    "skipNext" -> skipNext(result)
                    "skipPrevious" -> skipPrevious(result)
                    "seekTo" -> {
                        val positionMs = call.argument<Int>("positionMs")?.toLong()
                        if (positionMs != null) seekTo(positionMs, result)
                        else result.error("INVALID_ARG", "positionMs is required", null)
                    }
                    "getPlayerState" -> getPlayerState(result)
                    "getImage" -> {
                        val uri = call.argument<String>("imageUri")
                        if (uri != null) getImage(uri, result)
                        else result.error("INVALID_ARG", "imageUri is required", null)
                    }
                    else -> result.notImplemented()
                }
            }

        // EventChannel: Native → Flutter のストリーム (再生状態の変更通知)
        EventChannel(flutterEngine.dartExecutor.binaryMessenger, EVENT_CHANNEL)
            .setStreamHandler(object : EventChannel.StreamHandler {
                override fun onListen(arguments: Any?, events: EventChannel.EventSink?) {
                    playerStateEventSink = events
                    subscribeToPlayerState()
                }

                override fun onCancel(arguments: Any?) {
                    playerStateEventSink = null
                }
            })
    }

    // ──────────────────────────────────────
    // 接続 / 切断
    // ──────────────────────────────────────
    private fun connect(result: MethodChannel.Result) {
        val params = ConnectionParams.Builder(CLIENT_ID)
            .setRedirectUri(REDIRECT_URI)
            .showAuthView(true)
            .build()

        SpotifyAppRemote.connect(this, params, object : Connector.ConnectionListener {
            override fun onConnected(appRemote: SpotifyAppRemote) {
                spotifyAppRemote = appRemote
                Log.d(TAG, "Connected to Spotify")
                subscribeToPlayerState()
                result.success(true)
            }

            override fun onFailure(throwable: Throwable) {
                Log.e(TAG, "Connection failed", throwable)
                result.error("CONNECTION_FAILED", throwable.message, null)
            }
        })
    }

    private fun disconnect(result: MethodChannel.Result) {
        spotifyAppRemote?.let {
            SpotifyAppRemote.disconnect(it)
            spotifyAppRemote = null
        }
        result.success(true)
    }

    // ──────────────────────────────────────
    // 再生コントロール
    // ──────────────────────────────────────
    private fun play(uri: String, result: MethodChannel.Result) {
        ensureConnected(result) { remote ->
            remote.playerApi.play(uri)
                .setResultCallback { result.success(true) }
                .setErrorCallback { result.error("PLAY_FAILED", it.message, null) }
        }
    }

    private fun resume(result: MethodChannel.Result) {
        ensureConnected(result) { remote ->
            remote.playerApi.resume()
                .setResultCallback { result.success(true) }
                .setErrorCallback { result.error("RESUME_FAILED", it.message, null) }
        }
    }

    private fun pause(result: MethodChannel.Result) {
        ensureConnected(result) { remote ->
            remote.playerApi.pause()
                .setResultCallback { result.success(true) }
                .setErrorCallback { result.error("PAUSE_FAILED", it.message, null) }
        }
    }

    private fun skipNext(result: MethodChannel.Result) {
        ensureConnected(result) { remote ->
            remote.playerApi.skipNext()
                .setResultCallback { result.success(true) }
                .setErrorCallback { result.error("SKIP_FAILED", it.message, null) }
        }
    }

    private fun skipPrevious(result: MethodChannel.Result) {
        ensureConnected(result) { remote ->
            remote.playerApi.skipPrevious()
                .setResultCallback { result.success(true) }
                .setErrorCallback { result.error("SKIP_FAILED", it.message, null) }
        }
    }

    private fun seekTo(positionMs: Long, result: MethodChannel.Result) {
        ensureConnected(result) { remote ->
            remote.playerApi.seekTo(positionMs)
                .setResultCallback { result.success(true) }
                .setErrorCallback { result.error("SEEK_FAILED", it.message, null) }
        }
    }

    // ──────────────────────────────────────
    // 状態取得
    // ──────────────────────────────────────
    private fun getPlayerState(result: MethodChannel.Result) {
        ensureConnected(result) { remote ->
            remote.playerApi.playerState
                .setResultCallback { state -> result.success(playerStateToMap(state)) }
                .setErrorCallback { result.error("STATE_FAILED", it.message, null) }
        }
    }

    // ──────────────────────────────────────
    // アルバムアート取得
    // ──────────────────────────────────────
    private fun getImage(imageUriStr: String, result: MethodChannel.Result) {
        ensureConnected(result) { remote ->
            val imageUri = ImageUri(imageUriStr)
            remote.imagesApi.getImage(imageUri)
                .setResultCallback { bitmap ->
                    val stream = ByteArrayOutputStream()
                    bitmap.compress(Bitmap.CompressFormat.PNG, 100, stream)
                    result.success(stream.toByteArray())
                }
                .setErrorCallback {
                    result.error("IMAGE_FAILED", it.message, null)
                }
        }
    }

    // ──────────────────────────────────────
    // EventChannel: PlayerState ストリーム
    // ──────────────────────────────────────
    private fun subscribeToPlayerState() {
        spotifyAppRemote?.playerApi?.subscribeToPlayerState()?.setEventCallback { state ->
            runOnUiThread {
                playerStateEventSink?.success(playerStateToMap(state))
            }
        }
    }

    // ──────────────────────────────────────
    // ヘルパー
    // ──────────────────────────────────────
    private fun ensureConnected(
        result: MethodChannel.Result,
        action: (SpotifyAppRemote) -> Unit
    ) {
        val remote = spotifyAppRemote
        if (remote != null && remote.isConnected) {
            action(remote)
        } else {
            result.error("NOT_CONNECTED", "Spotify App Remote is not connected", null)
        }
    }

    private fun playerStateToMap(state: PlayerState): Map<String, Any?> {
        return mapOf(
            "trackName" to (state.track?.name),
            "artistName" to (state.track?.artist?.name),
            "albumName" to (state.track?.album?.name),
            "trackUri" to (state.track?.uri),
            "imageUri" to (state.track?.imageUri?.raw),
            "durationMs" to (state.track?.duration ?: 0L),
            "positionMs" to state.playbackPosition,
            "isPaused" to state.isPaused,
        )
    }

    override fun onStop() {
        super.onStop()
        // バックグラウンドに行っても切断しない（音楽再生を維持するため）
        // 必要に応じて disconnect を呼ぶ
    }

    override fun onDestroy() {
        super.onDestroy()
        spotifyAppRemote?.let { SpotifyAppRemote.disconnect(it) }
    }
}
