import 'package:flutter/material.dart';

class InfoCard extends StatelessWidget{
  final String title;
  final String subtitle;
  final Color color;
  final IconData icon;

  const InfoCard({
    super.key,
    required this.title,
    required this.subtitle,
    required this.color,
    required this.icon,
  });

  @override
  Widget build(BuildContext context){
    return Container(
      padding: const EdgeInsets.all(20),
    );
  }
}