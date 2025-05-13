import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:menuapp/Model/ban.dart';
import 'package:menuapp/Model/chitietdatban.dart';
import 'package:menuapp/Model/monAns.dart';
import 'package:menuapp/View/payBill.dart';
import 'package:provider/provider.dart';

class OrderFooter extends StatelessWidget {
  final VoidCallback chotMonAn;
  final Ban ban;
  final Future<List<ChiTietDatBan>>? selectedItems;

  const OrderFooter({
    Key? key,
    required this.chotMonAn,
    required this.ban,
    required this.selectedItems,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<ChiTietDatBan>>(
      future: selectedItems,
      builder: (context, snapshot) {
        final selectedItems = snapshot.data ?? [];
        double totalAmount = selectedItems.fold(
            0, (sum, item) => sum + (item.soLuong * item.gia));
        bool check = selectedItems.every((item) => item.trangThai == true);
        return Container(
          padding: const EdgeInsets.all(16.0),
          color: Colors.orange[100],
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.end,
            children: [
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text('Tên bàn: ${ban.tenBan}',
                      style: const TextStyle(
                          fontSize: 18, fontWeight: FontWeight.bold)),
                  Text(
                      'Tổng tiền: ${NumberFormat.currency(locale: 'vi', symbol: '₫').format(totalAmount)}',
                      style: const TextStyle(
                          fontSize: 18, fontWeight: FontWeight.bold)),
                ],
              ),
              const SizedBox(height: 8),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  ElevatedButton(
                    onPressed: chotMonAn,
                    style:
                        ElevatedButton.styleFrom(backgroundColor: Colors.blue),
                    child: const Text('Chốt Món'),
                  ),
                  ElevatedButton(
                    onPressed: () {
                      if (check) {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) =>
                                Paybill(selectedItems: selectedItems, ban: ban),
                          ),
                        );
                      } else {
                        ScaffoldMessenger.of(context).showSnackBar(
                          const SnackBar(
                              content:
                                  Text('Có món ăn chưa lên')),
                        );
                      }
                    },
                    style:
                        ElevatedButton.styleFrom(backgroundColor: Colors.red),
                    child: const Text('Thanh Toán',
                        style: TextStyle(color: Colors.black)),
                  ),
                ],
              ),
            ],
          ),
        );
      },
    );
  }
}
