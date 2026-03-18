import 'package:flutter/material.dart';
import 'package:ui_learn/theme/app_theme.dart';

class Header extends StatelessWidget{
  final String label;
  final String subLabel;

  const Header({
    super.key,
    required this.label,
    required this.subLabel
  });


  @override
  Widget build(BuildContext context){
        return Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            Column(
              mainAxisAlignment: MainAxisAlignment.center,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  label,
                  style: AppTextStyles.appBarTitle,
                ),
                Text(
                  subLabel,
                  style: TextStyle(
                    fontSize: 10,
                  ),
                )
              ],
            ),
            Image.asset('assets/images/icons/ourvibe_logo.png',height: 80,width: 80,)
          ],
        );
  }
}