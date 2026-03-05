package com.example.native_lib_test

import io.flutter.embedding.android.FlutterActivity
import io.flutter.embedding.engine.FlutterEngine
import io.flutter.plugin.common.MethodChannel
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken

class MainActivity : FlutterActivity(){
    private val CHANNEL = "com.example.native_lib_test/gson"

    override fun configureFlutterEngine(flutterEngine:FlutterEngine){
        super.configureFlutterEngine(flutterEngine)

                MethodChannel(flutterEngine.dartExecutor.binaryMessenger, CHANNEL)
            .setMethodCallHandler { call, result ->
                when (call.method) {
                    "testGson" -> {
                        try {
                            val gson = Gson()

                            // Flutter から受け取った引数を取得
                            val name = call.argument<String>("name") ?: "unknown"
                            val language = call.argument<String>("language") ?: "unknown"
                            val level = call.argument<Int>("level") ?: 0

                            // データクラスに詰める
                            val user = UserData(
                                name = name,
                                language = language,
                                level = level,
                                greeting = "こんにちは、${name}さん！"
                            )

                            // Gson でシリアライズ (Object → JSON文字列)
                            val json = gson.toJson(user)

                            // Gson でデシリアライズ (JSON文字列 → Object)
                            val parsed: UserData = gson.fromJson(json, UserData::class.java)

                            // 結果をまとめて返す
                            val response = buildString {
                                appendLine("=== Gson テスト結果 ===")
                                appendLine()
                                appendLine("【シリアライズ】")
                                appendLine(json)
                                appendLine()
                                appendLine("【デシリアライズ】")
                                appendLine("name: ${parsed.name}")
                                appendLine("language: ${parsed.language}")
                                appendLine("level: ${parsed.level}")
                                appendLine("greeting: ${parsed.greeting}")
                                appendLine()
                                appendLine("Gson version: OK ✅")
                            }

                            result.success(response)
                        } catch (e: Exception) {
                            result.error("GSON_ERROR", e.message, null)
                        }
                    }
                    else -> result.notImplemented()
                }
            }
    }
}


data class UserData(
    val name: String,
    val language: String,
    val level: Int,
    val greeting: String
)