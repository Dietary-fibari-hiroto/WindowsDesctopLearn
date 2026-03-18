import 'package:go_router/go_router.dart';

import 'package:flutter/material.dart';
import 'package:ui_learn/widgets/Widgets.dart';
import '../theme/app_theme.dart';

class CenterButtonScreen extends StatelessWidget{
  final String? message;
  const CenterButtonScreen({super.key,this.message});

  @override
  Widget build(BuildContext context){
    return Scaffold(
      appBar:AppBar(
        title: 
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Column(
                mainAxisAlignment:MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text('Screen 1 - Center',style:AppTextStyles.appBarTitle),
                  Text('センターを意識した一つ目のスクリーン',style:TextStyle(fontSize:10,color:Color(0xFFCCCCCC)))
                ],
              ),
              Image.asset('assets/images/icons/ourvibe_logo.png',height: 100,width:100),
              
            ],
          ),
        backgroundColor:Color(0xFF1E1E1E),
        centerTitle: true,
      ),
      body:Center(
        child:Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text('テキスト'),
          if (message != null) MessageBanner(message: message!),

            ElevatedButton(
              onPressed: () => context.go('/horizontal'),
              style:AppButtonStyles.pill(
                backgroundColor:AppColors.screen1Accent,
              ),
              child:const Text('横並びページへ →')
            )
          ],
        )
      )
    );
  }
}