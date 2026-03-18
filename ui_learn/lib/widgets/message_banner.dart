import 'package:flutter/material.dart';

import '../theme/app_theme.dart';

/// 受け取ったメッセージを表示するバナー（Screen 1 で使用）
class MessageBanner extends StatelessWidget {
  final String message;

  const MessageBanner({super.key, required this.message});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 16),
      margin: const EdgeInsets.only(bottom: 24),
      decoration: BoxDecoration(
        color: AppColors.screen1Card,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: AppColors.screen1Accent, width: 2),
        boxShadow: [
          BoxShadow(
            color: AppColors.screen1Accent.withValues(alpha: 0.3),
            blurRadius: 15,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: Text(
        '受け取ったメッセージ:\n$message',
        textAlign: TextAlign.center,
        style: const TextStyle(
          fontSize: 18,
          color: AppColors.screen1Accent,
          fontWeight: FontWeight.w600,
          letterSpacing: 0.5,
        ),
      ),
    );
  }
}