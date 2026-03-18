import 'package:flutter/material.dart';

class IconCardData{
  final String label;
  final Color color;
  final IconData icon;

  const IconCardData({
    required this.label,
    required this.color,
    required this.icon,
  });
}

class IconCard extends StatelessWidget {
  final String label;
  final Color color;
  final IconData icon;

  const IconCard({
    super.key,
    required this.label,
    required this.color,
    required this.icon,
  });

  factory IconCard.fromDto(IconCardData data){
    return IconCard(label: data.label, color: data.color, icon: data.icon);
  }

  @override
  Widget build(BuildContext context){
    return Container(
      width:90,
      height: 90,
      decoration: BoxDecoration(
        color:color.withValues(alpha: 0.15),
        borderRadius: BorderRadius.circular(16),
        border:Border.all(color:color,width:2),
      ),
      child:Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Icon(icon,color: color,size:32),

          const SizedBox(height: 4),
          Text(
            label,
            style:TextStyle(
              color:color,
              fontSize: 18,
              fontWeight: FontWeight.bold
            )
          )
        ],
      )
    );
  }

  
}