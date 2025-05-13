// import 'package:flutter/material.dart';
// import 'package:http/http.dart' as http;
// import 'package:menuapp/Model/ban.dart';
// import 'package:menuapp/View/widget/tablecar.dart';
// import 'package:menuapp/controller/ban_controller.dart';
// import 'dart:convert';
// import 'listOrders.dart';

// class OrdersScreen extends StatefulWidget {
//   const OrdersScreen({Key? key}) : super(key: key);

//   @override
//   State<OrdersScreen> createState() => _OrdersScreenState();
// }

// class _OrdersScreenState extends State<OrdersScreen> {
//   //final BanController _banController = BanController();
//   List<Ban> tables = [];

//   @override
//   void initState() {
//     super.initState();
//     // Call the function to fetch data after the frame is built
//     WidgetsBinding.instance.addPostFrameCallback((_) {
//       dsban();
//     });
//   }

//   Future<void> dsban() async {
//     // goi ham lay ds ban 
//     tables = await banController.dsban(); 
//     setState(() {}); 
//   }

//  @override
//   Widget build(BuildContext context) {
//     return Scaffold(
//       backgroundColor: const Color(0xFFF0F0F0), 
//       appBar: AppBar(
//         title: const Text(
//           'Quản lý bàn ăn',
//           style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
//         ),
//         centerTitle: true,
//         backgroundColor: const Color(0xFF1E88E5),
//         elevation: 0,
//       ),
//       body: Padding(
//         padding: const EdgeInsets.all(16.0),
//         child: Column(
//           children: [
//             SizedBox(
//               height: MediaQuery.of(context).size.height * 0.5,
//               child: GridView.builder(
//                 physics: const NeverScrollableScrollPhysics(),
//                 shrinkWrap: true,
//                 itemCount: tables.length,
//                 gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
//                   crossAxisCount: 3,
//                   crossAxisSpacing: 10,
//                   mainAxisSpacing: 10,
//                 ),
//                 itemBuilder: (context, index) {
//                   return TableCard(
//                     ban: tables[index],
//                     onTap: () {
//                       Navigator.push(
//                         context,
//                         MaterialPageRoute(
//                           builder: (context) => ListOrders(ban: tables[index]),
//                         ),
//                       );
//                     },
//                   );
//                 },
//               ),
//             ),
//           ],
//         ),
//       ),
//     );
//   }
// }