import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:ui_learn/theme/app_theme.dart';
import 'package:ui_learn/widgets/Header.dart';
import 'package:ui_learn/widgets/icon_card.dart';



class HorizontalScreen extends StatelessWidget{
  const HorizontalScreen({super.key});


  static const cardItems=[
    IconCardData(label: 'A', color: AppColors.cardRed, icon:Icons.star),
    IconCardData(label: 'B', color:AppColors.cardTeal, icon: Icons.favorite),
    IconCardData(label: 'C', color:AppColors.cardYellow, icon: Icons.bolt)
  ];

  @override
  Widget build(BuildContext context){
    return Scaffold(
      appBar:AppBar(title:Header(label: 'Screen 2 - Row',subLabel: '横並びの画面')),
      body:Padding(
        padding:const EdgeInsets.all(24),
        child: Column(
          children: [
            const Text('Row = CSS の flex-direction: row',style:AppTextStyles.sectionLabel),
            const SizedBox(height:24),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children:[
                ...cardItems.map((item)=> IconCard.fromDto(item)).toList(),
              ]
            ),
            const SizedBox(height:32),

            const Text('Expanded = CSS の flex: 1',style:AppTextStyles.sectionLabel),
            const SizedBox(height:12),

            Row(
              children: [
                Expanded(
                  flex:2,
                  child:Container(
                    height: 80,
                    margin:const EdgeInsets.only(right:8),
                    decoration: BoxDecoration(
                      gradient: const LinearGradient(
                        colors: AppColors.gradientPurple,
                        begin:Alignment.topLeft,
                        end:Alignment.bottomRight
                      ),
                      borderRadius: BorderRadius.circular(12),
                    ),
                    alignment: Alignment.center,
                    child: const Text('flex: 2',
                        style: TextStyle(
                            color: Colors.white,
                            fontSize: 16,
                            fontWeight: FontWeight.bold)
                    ),
                  ),
                ),

                Expanded(
                  flex: 1,
                  child: Container(
                    height: 80,
                    decoration: BoxDecoration(
                      gradient:
                          const LinearGradient(colors: AppColors.gradientPink),
                      borderRadius: BorderRadius.circular(12),
                    ),
                    alignment: Alignment.center,
                    child: const Text('flex: 1',
                        style: TextStyle(
                            color: Colors.white,
                            fontSize: 16,
                            fontWeight: FontWeight.bold)),
                  ),
                ),


              ],
            ),

            const SizedBox(height: 32),

            // --- Wrap デモ ---
            const Text('Wrap = CSS の flex-wrap: wrap',
                style: AppTextStyles.sectionLabel),
            const SizedBox(height: 12),

            Wrap(
              spacing: 8,
              runSpacing: 8,
              children: List.generate(
                7,
                (i) => Container(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                  decoration: BoxDecoration(
                    color: Colors.primaries[i % Colors.primaries.length]
                        .withValues(alpha: 0.7),
                    borderRadius: BorderRadius.circular(20),
                  ),
                  child: Text('Tag ${i + 1}',
                      style:
                          const TextStyle(color: Colors.white, fontSize: 13)),
                ),
              ),
            ),

            const Spacer(),

            SizedBox( 
              width:double.infinity,
              child: ElevatedButton(
                onPressed: () => context.go('/vertical'),
                style: AppButtonStyles.block(
                  backgroundColor: AppColors.screen2Accent,
                  foregroundColor: AppColors.screen2Bg,
                ),
                child: const Text('縦並びページへ ↓'),
              
              ), 
            )

          ],
        ),
      ),
    );
  }
}