import 'package:go_router/go_router.dart';
import 'package:ui_learn/screens/horizontal_screen.dart';
import 'package:ui_learn/screens/vertical_screen.dart';

import './screens/center_button_screen.dart';

final GoRouter appRouter = GoRouter(
  initialLocation:'/',
  routes: [
    GoRoute(
      path: '/',
      builder:(context,state){
        final message = state.uri.queryParameters['message'];
        return CenterButtonScreen(message:message);
      }
    ),
    GoRoute(
      path:'/horizontal',
      builder: (context,state) => const HorizontalScreen(),
    ),
    GoRoute(
      path:'/vertical',
      builder:(context,state)=>const VerticalScreen(),
    )
  ]
);