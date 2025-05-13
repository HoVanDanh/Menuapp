import 'dart:io';

import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:menuapp/Model/monAns.dart';
import 'package:menuapp/View/widget/textsoluong.dart';

class CardProduct extends StatelessWidget {
  final MonAn monAn;
  final int quantity;
  final VoidCallback onIncrement;
  final VoidCallback onDecrement;

  const CardProduct({
    Key? key,
    required this.monAn,
    required this.quantity,
    required this.onIncrement,
    required this.onDecrement,
  }) : super(key: key);

  String chuyenDoiDangTien(int giaTien) {
    return NumberFormat.currency(locale: 'vi_VN', symbol: '₫', decimalDigits: 0)
        .format(giaTien);
  }

  @override
  Widget build(BuildContext context) {
    return Card(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(12),
      ),
      elevation: 4,
      margin: const EdgeInsets.symmetric(vertical: 8, horizontal: 8),
      color: Colors.orange[50],
      child: Padding(
        padding: const EdgeInsets.all(12.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            // Hình ảnh sản phẩm
            Expanded(
              child: ClipRRect(
                  borderRadius: BorderRadius.circular(8),
                  child:
                      Image.network('http://192.168.226.1:801/${monAn.img}')),
            ),
            const SizedBox(height: 10),
            Text(
              monAn.tenMon,
              style: const TextStyle(
                fontSize: 16,
                fontWeight: FontWeight.bold,
              ),
              textAlign: TextAlign.center,
            ),
            const SizedBox(height: 8),
            // Giá sản phẩm
            Text(
              chuyenDoiDangTien(monAn.gia),
              style: const TextStyle(
                fontSize: 16,
                color: Colors.red,
              ),
            ),
            const SizedBox(height: 10),
            // Tăng giảm số lượng
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                IconButton(
                  icon: const Icon(Icons.remove_circle_outline,
                      color: Colors.red),
                  onPressed: onDecrement,
                ),
                Text(
                  '$quantity',
                  style: const TextStyle(fontSize: 18),
                ),
                IconButton(
                  icon:
                      const Icon(Icons.add_circle_outline, color: Colors.green),
                  onPressed: onIncrement,
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
