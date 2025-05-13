import 'package:flutter/material.dart';
import 'package:menuapp/Model/ban.dart';

// TableCard widget
class TableCard extends StatelessWidget {
  final Ban ban;
  final VoidCallback onTap;

  const TableCard({
    Key? key,
    required this.ban,
    required this.onTap,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        decoration: BoxDecoration(
          color: ban.trangThai
              ? Colors.red[300]
              : Colors.green[300], // Dựa trên trạng thái
          borderRadius: BorderRadius.circular(10),
          border: Border.all(color: Colors.grey),
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon( Icons.table_restaurant,
              color: Colors.white,
              size: 50,
            ),
            const SizedBox(height: 8),
            Text(
              "ban " + ban.tenBan,
              style: const TextStyle(
                fontSize: 16,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
            const SizedBox(height: 4),
            Text(
              ban.trangThai ? 'Có Khách' : 'Bàn trống',
              style: const TextStyle(
                fontSize: 14,
                color: Colors.white,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
