import 'package:flutter/material.dart';
import 'package:menuapp/Model/chitietdatban.dart';

class CardSelectedItem extends StatelessWidget {
  final ChiTietDatBan chiTietDatBan;
  final VoidCallback onDelete;
  final VoidCallback onSusscec;

  const CardSelectedItem({
    Key? key,
    required this.chiTietDatBan,
    required this.onDelete,
    required this.onSusscec,
  }) : super(key: key);
  void _showDeleteConfirmationDialog(BuildContext context) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text("Xác nhận hủy món ăn"),
          content: const Text("Bạn có chắc chắn muốn hủy món ăn này?"),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop(); // Đóng dialog
              },
              child: const Text("Hủy"),
            ),
            TextButton(
              onPressed: () {
                Navigator.of(context).pop();
                onDelete();
              },
              child: const Text("Xóa"),
            ),
          ],
        );
      },
    );
  }

  void _showXacNhanRaMon(BuildContext context) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text("Xác nhận món ăn đã giao"),
          content: const Text("Món ăn này đã hoàn thành"),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop(); // Đóng dialog
              },
              child: const Text("Hủy"),
            ),
            TextButton(
              onPressed: () {
                Navigator.of(context).pop();
                onSusscec();
              },
              child: const Text("Xác nhận"),
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: const EdgeInsets.symmetric(vertical: 8.0, horizontal: 16.0),
      decoration: BoxDecoration(
        border: Border.all(color: Colors.grey.shade300),
        borderRadius: BorderRadius.circular(8.0),
        color: Colors.orange[50],
      ),
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  chiTietDatBan.tenMonAn,
                  style: const TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                Row(
                  children: [
                    IconButton(
                      icon: const Icon(Icons.delete, color: Colors.red),
                      onPressed: chiTietDatBan.trangThai== true ? null : () => _showDeleteConfirmationDialog(context),
                    ),
                  ],
                )
              ],
            ),
            const SizedBox(height: 8),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text('Số lượng: ${chiTietDatBan.soLuong}'),
                Text(
                  'Giá: ${chiTietDatBan.gia}',
                  style: const TextStyle(color: Colors.red),
                ),
              ],
            ),
            const SizedBox(height: 8),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  chiTietDatBan.trangThai
                      ? "Đã lên món"
                      : (chiTietDatBan.timeconlai == 0
                          ? "Sẵn sàng"
                          : "Thời gian ra món: ${chiTietDatBan.timeconlai} phút"),
                  style: TextStyle(
                    fontSize: 16,
                    color: chiTietDatBan.timeconlai == 0 || chiTietDatBan.trangThai == true
                        ? Colors.green
                        : Colors.black,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                IconButton(
                  icon: const Icon(Icons.food_bank, color: Colors.green),
                  onPressed: () => _showXacNhanRaMon(context),
                ),
              ],
            )
          ],
        ),
      ),
    );
  }
}
