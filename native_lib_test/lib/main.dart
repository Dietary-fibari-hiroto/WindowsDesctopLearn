import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'player_screen.dart';
import 'spotify_service.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
  return ChangeNotifierProvider(
    create:(_)=> SpotifyService(),
    child:MaterialApp(
      title:'SpotifySample',
      debugShowCheckedModeBanner: false,
      theme: ThemeData.dark().copyWith(
        scaffoldBackgroundColor: const Color(0xFF121212),
      ),
      home:const PlayerScreen(),
      )  
    );
  }
}


