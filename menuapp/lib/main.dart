import 'dart:io';
import 'package:menuapp/provider/accountProvider.dart';
import 'package:provider/provider.dart';
import 'package:flutter/material.dart';
import 'package:menuapp/View/homepage/homePage.dart';
import 'package:menuapp/View/loginPage.dart';
import 'package:menuapp/View/productPage/menuScreen.dart';
import 'package:menuapp/View/Orderpage/ordersScreen.dart';
import 'package:menuapp/View/payBill.dart';
import 'package:menuapp/View/profilesetting.dart';

void main() {
   HttpOverrides.global = MyHttpOverrides();
  runApp( 
    ChangeNotifierProvider(
      create: (context) => accountnprovider(),
      child: const MyApp(),
    ),
  );
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      initialRoute: '/',
      routes: {
        '/HomePage': (context) => const HomePage(),
        '/': (context) => const LoginPage(),
        '/MenuScreen': (context) => const MenuScreen(),
       // '/OrdersScreen': (context) => const OrdersScreen(),
       // '/Paybill': (context) => const Paybill(),
       // '/ProfileEditScreen': (context) => const ProfileEditScreen(),
      },
    );
  }
}
// khởi tạo cho phép truy cập tới webapi 
 class MyHttpOverrides extends HttpOverrides{
  @override
  HttpClient createHttpClient(SecurityContext? context){
    return super.createHttpClient(context)
      ..badCertificateCallback = (X509Certificate cert, String host, int port)=> true;
  }
}