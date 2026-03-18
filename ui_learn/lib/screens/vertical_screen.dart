
import '../theme/app_theme.dart';
import '../widgets/widgets.dart';
import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

class VerticalScreen extends StatelessWidget{
  const VerticalScreen({super.key});

  @override
  Widget build(BuildContext context){

return Scaffold(
      backgroundColor: AppColors.screen3Bg,
      appBar: AppBar(
        title: Text(
          'Screen 3 - Column (縦並び)',
          style: AppTextStyles.appBarTitle.copyWith(
            color: AppColors.screen3Accent,
          ),
        ),
        backgroundColor: AppColors.screen3AppBar,
        centerTitle: true,
        iconTheme: const IconThemeData(color: AppColors.screen3Accent),
      ),
      body: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            const InfoCard(
              title: 'Column',
              subtitle: 'flex-direction: column',
              color: AppColors.screen3Accent,
              icon: Icons.view_agenda,
            ),
            const SizedBox(height: 16),

            const InfoCard(
              title: 'MainAxis',
              subtitle: '↕ 縦方向 (justify-content)',
              color: AppColors.screen3Teal,
              icon: Icons.swap_vert,
            ),
            const SizedBox(height: 16),

            const InfoCard(
              title: 'CrossAxis',
              subtitle: '↔ 横方向 (align-items)',
              color: AppColors.screen3Pink,
              icon: Icons.swap_horiz,
            ),
            const SizedBox(height: 40),

            // --- 値を渡して最初に戻る ---
            ElevatedButton(
              onPressed: () {
                final now = DateTime.now();
                final timeStr =
                    '${now.hour}:${now.minute.toString().padLeft(2, '0')}:'
                    '${now.second.toString().padLeft(2, '0')}';
                context.go(
                  '/?message=3ページ目から戻ってきたよ！ (時刻: $timeStr)',
                );
              },
              style: AppButtonStyles.block(
                backgroundColor: AppColors.screen3Accent,
                foregroundColor: AppColors.screen3Bg,
              ),
              child: const Text('最初のページへ（値を渡す） ↩'),
            ),

            const SizedBox(height: 12),
            const Text(
              'ボタンを押すと、現在時刻をクエリパラメータで\n1ページ目に渡します',
              textAlign: TextAlign.center,
              style: TextStyle(fontSize: 13, color: Colors.white38, height: 1.6),
            ),
          ],
        ),
      ),
    );
  }
}