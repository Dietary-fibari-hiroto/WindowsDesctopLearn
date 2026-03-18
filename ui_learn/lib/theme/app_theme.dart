import 'package:flutter/material.dart';


sealed class AppColors{
  // Screen 1
  static const screen1Bg = Color(0xFF1A1A2E);
  static const screen1AppBar = Color(0xFF16213E);
  static const screen1Accent = Color(0xFFE94560);
  static const screen1Card = Color(0xFF0F3460);

  // Screen 2
  static const screen2Bg = Color(0xFF0A0E21);
  static const screen2AppBar = Color(0xFF1C1C3A);
  static const screen2Accent = Color(0xFF00D2FF);

  // Screen 3
  static const screen3Bg = Color(0xFF1B0A2A);
  static const screen3AppBar = Color(0xFF2D1B4E);
  static const screen3Accent = Color(0xFFBB86FC);
  static const screen3Teal = Color(0xFF03DAC6);
  static const screen3Pink = Color(0xFFCF6679);

  // Gradients
  static const gradientPurple = [Color(0xFF667EEA), Color(0xFF764BA2)];
  static const gradientPink = [Color(0xFFF093FB), Color(0xFFF5576C)];

  // Cards (Screen 2)
  static const cardRed = Color(0xFFFF6B6B);
  static const cardTeal = Color(0xFF4ECDC4);
  static const cardYellow = Color(0xFFFFE66D);

  static const primaryColor = Color(0xFF4C86AF);
  static const primaryTextColor=Color(0xFFEFEEFF);
}

sealed class AppTheme{
  static ThemeData get dark{
    return ThemeData(
      colorScheme:ColorScheme.fromSeed(
        seedColor: Colors.deepPurple,
        brightness: Brightness.dark,
      ),
      textTheme: const TextTheme(
        bodyMedium: TextStyle(
          fontSize: 16,
          fontFamily: 'NotoSansJP',
          color: AppColors.primaryTextColor
        ),
        titleLarge: const TextStyle(
          fontFamily: 'NotoSerifJP',
          fontSize:32,
          color: AppColors.primaryTextColor
        )
      ),
      scaffoldBackgroundColor:AppColors.primaryColor,
      useMaterial3:true,

      appBarTheme: const AppBarTheme(
        backgroundColor:Color(0xFF1E1E1E),
      )
    );
  }
}


sealed class AppTextStyles{
  static const appBarTitle = TextStyle(
    fontSize: 20,
    fontWeight: FontWeight.bold,
    color:Colors.white,
  );

  static const sectionLabel = TextStyle(
    fontSize: 14,
    color:Colors.white54,
    fontStyle: FontStyle.italic,
  );

  static const buttonText = TextStyle(
    fontSize: 18,
    fontWeight: FontWeight.bold,
  );
}


sealed class AppButtonStyles{
  static ButtonStyle pill({
    required Color backgroundColor,
    Color foregroundColor = Colors.white,
  }){
    return ElevatedButton.styleFrom(
      backgroundColor: backgroundColor,
      foregroundColor:foregroundColor,
      padding:const EdgeInsets.symmetric(horizontal:48,vertical:16),
      textStyle: AppTextStyles.buttonText,
      shape:RoundedRectangleBorder(
        borderRadius:BorderRadius.circular(30),
      ),
      elevation: 8,
      shadowColor: backgroundColor,
    );
  }


  static ButtonStyle block({
    required Color backgroundColor,
    Color? foregroundColor,
  }){
    return ElevatedButton.styleFrom(
      backgroundColor: backgroundColor,
      foregroundColor: foregroundColor ?? Colors.white,
      padding: const EdgeInsets.symmetric(vertical: 16),
      textStyle: AppTextStyles.buttonText,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(12),
      ),
      elevation: 12,
      shadowColor: backgroundColor,
    );
  }
}


