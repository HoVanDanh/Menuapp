import 'package:flutter/material.dart';

class GridItemWidget extends StatelessWidget {
  final IconData icon;
  final String label;
  final Widget screen;

  // Constructor để nhận các tham số icon, label và màn hình
  const GridItemWidget({
    super.key,
    required this.icon,
    required this.label,
    required this.screen,
  });

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: () {
        // Khi người dùng nhấn vào, chuyển tới màn hình tương ứng
        Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => screen),
        );
      },
      child: Container(
        margin: const EdgeInsets.all(10),
        decoration: BoxDecoration(
          color: Colors.orange[100],
          borderRadius: BorderRadius.circular(10), // Bo góc của các ô
          boxShadow: const [
            BoxShadow(
              color: Colors.black26,
              blurRadius: 5,
              offset: Offset(2, 2), // Độ lệch của bóng
            ),
          ],
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(
              icon,
              size: 50,
              color: Colors.orange[600],
            ),
            const SizedBox(height: 10),
            Text(
              label,
              style: const TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.bold,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
