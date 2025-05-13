// import 'package:flutter/material.dart';
// import 'package:menuapp/Model/chitietdatban.dart';

// class OrderDetailDialog extends StatefulWidget {
//   final ChiTietDatBan chiTietDatBan;
//   final VoidCallback onDelete;
//   final ValueChanged<int> onQuantityChanged;

//   const OrderDetailDialog({
//     Key? key,
//     required this.chiTietDatBan,
//     required this.onDelete,
//     required this.onQuantityChanged,
//   }) : super(key: key);

//   @override
//   _OrderDetailDialogState createState() => _OrderDetailDialogState();
// }

// class _OrderDetailDialogState extends State<OrderDetailDialog> {
//   late TextEditingController _quantityController;

//   @override
//   void initState() {
//     super.initState();
//     _quantityController = TextEditingController(text: widget.chiTietDatBan.soLuong.toString());
//   }

//   @override
//   void dispose() {
//     _quantityController.dispose();
//     super.dispose();
//   }

//   void _updateQuantity() {
//     final newQuantity = int.tryParse(_quantityController.text) ?? widget.chiTietDatBan.soLuong;
//     widget.onQuantityChanged(newQuantity);
//     Navigator.pop(context);
//   }

//   @override
//   Widget build(BuildContext context) {
//     return AlertDialog(
//       title: Text('Chi tiết đặt bàn: ${widget.chiTietDatBan.tenMonAn}'),
//       content: Column(
//         mainAxisSize: MainAxisSize.min,
//         children: [
//           Text('Tên món ăn: ${widget.chiTietDatBan.tenMonAn}'),
//           const SizedBox(height: 8),
//           Row(
//             mainAxisAlignment: MainAxisAlignment.spaceBetween,
//             children: [
//               const Text('Số lượng:'),
//               SizedBox(
//                 width: 50,
//                 child: TextField(
//                   controller: _quantityController,
//                   keyboardType: TextInputType.number,
//                   decoration: const InputDecoration(
//                     border: OutlineInputBorder(),
//                   ),
//                 ),
//               ),
//             ],
//           ),
//           const SizedBox(height: 8),
//         ],
//       ),
//       actions: [
//         TextButton(
//           onPressed: () => Navigator.pop(context),
//           child: const Text('Hủy'),
//         ),
//         ElevatedButton(
//           onPressed: () {
//             _updateQuantity(); 
//           },
//           child: const Text('Lưu'),
//         ),
//         ElevatedButton(
//           onPressed: () {
//             Navigator.pop(context); // Đóng dialog
//             widget.onDelete(); // Xóa món ăn
//           },
//           child: const Text('Xóa'),
//         ),
//       ],
//     );
//   }
// }
