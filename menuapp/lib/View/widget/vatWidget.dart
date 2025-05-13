import 'package:flutter/material.dart';

class VatWidget extends StatefulWidget {
  const VatWidget({Key? key}) : super(key: key);

  @override
  _VatWidgetState createState() => _VatWidgetState();
}

class _VatWidgetState extends State<VatWidget> {
  bool isVatChecked = false; // Trạng thái của checkbox
  final TextEditingController tencongtyController = TextEditingController();
  final TextEditingController masothueController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        // Dòng text "VAT" và checkbox
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            const Text(
              "VAT",
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
            ),
            Checkbox(
              value: isVatChecked,
              onChanged: (bool? value) {
                setState(() {
                  isVatChecked = value ?? false;
                });
              },
            ),
          ],
        ),
        // Hiển thị TextField khi checkbox được chọn
        if (isVatChecked) ...[
          const SizedBox(height: 10),
          TextField(
            controller: tencongtyController,
            decoration: const InputDecoration(
              labelText: 'Tên Công Ty',
              border: OutlineInputBorder(),
            ),
          ),
          const SizedBox(height: 10),
          TextField(
            controller: masothueController,
            decoration: const InputDecoration(
              labelText: 'Mã Số Thuế',
              border: OutlineInputBorder(),
            ),
          ),
        ],
      ],
    );
  }

  @override
  void dispose() {
    // Giải phóng controller khi widget bị hủy
    tencongtyController.dispose();
    masothueController.dispose();
    super.dispose();
  }
}
