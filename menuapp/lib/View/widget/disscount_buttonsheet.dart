import 'package:flutter/material.dart';
import 'package:menuapp/Model/chietkhau.dart';
import 'package:menuapp/controller/chietkhau_controller.dart';

class DiscountInputWidget extends StatefulWidget {
  final TextEditingController chietKhauController;
  final Function(String) onDiscountCodeChanged; // Thêm callback

  DiscountInputWidget({
    required this.chietKhauController,
    required this.onDiscountCodeChanged, // Nhận vào callback
  });

  @override
  _DiscountInputWidgetState createState() => _DiscountInputWidgetState();
}

class _DiscountInputWidgetState extends State<DiscountInputWidget> {
  String? selectedDiscountCode; // Lưu mã chiết khấu đã chọn

  void _openDiscountSheet() async {
    // Mở BottomSheet để chọn mã giảm giá
    final selectedDiscount = await showModalBottomSheet<Map<String, dynamic>>(
      context: context,
      builder: (BuildContext context) {
        return DiscountSheet();
      },
    );

    // Cập nhật TextField và hiển thị mã chiết khấu đã chọn
    if (selectedDiscount != null) {
      setState(() {
        // Hiển thị giá trị chiết khấu trong TextField
        widget.chietKhauController.text = selectedDiscount['giaTri'].toString();
        // Cập nhật mã chiết khấu và gọi callback để thông báo thay đổi
        selectedDiscountCode = selectedDiscount['maChietKhau'];
        widget.onDiscountCodeChanged(selectedDiscountCode!); // Gọi callback
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Expanded(
              child: TextField(
                controller: widget.chietKhauController,
                readOnly: true,
                decoration: const InputDecoration(
                  labelText: 'Giá trị chiết khấu',
                  border: OutlineInputBorder(),
                ),
                keyboardType: TextInputType.number,
              ),
            ),
            IconButton(
              icon: Icon(Icons.discount),
              onPressed: _openDiscountSheet,
            ),
          ],
        ),
      ],
    );
  }
}

class DiscountSheet extends StatefulWidget {
  @override
  _DiscountSheetState createState() => _DiscountSheetState();
}

class _DiscountSheetState extends State<DiscountSheet> {
  late Future<List<ChietKhau>> _chietKhauList;

  @override
  void initState() {
    super.initState();
    _chietKhauList = ChietkhauController().fetchChietKhaus();
  }

  bool _isValidDiscount(ChietKhau discount) {
    final now = DateTime.now();
    return discount.ngayBatDau.isBefore(now) && discount.ngayKetThuc.isAfter(now) && discount.soLuong>0;
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<List<ChietKhau>>(
      future: _chietKhauList,
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return Center(child: CircularProgressIndicator());
        } else if (snapshot.hasError) {
          return Center(child: Text('Lỗi: ${snapshot.error}'));
        } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
          return Center(child: Text("Không có chiết khấu nào"));
        }

        final chietKhaus = snapshot.data!;
        return ListView.builder(
          itemCount: chietKhaus.length,
          itemBuilder: (context, index) {
            final discount = chietKhaus[index];
            final isValid = _isValidDiscount(discount);

            return ListTile(
              title: Text(
                '${discount.maChietKhau} | còn: ${discount.soLuong}',
                style: TextStyle(color: isValid ? Colors.green : Colors.red),
              ),
              subtitle: Text('${discount.giaTriChietKhau} ₫'),
              trailing: Text(
                isValid ? 'Còn hiệu lực' : 'Hết hiệu lực',
                style: TextStyle(color: isValid ? Colors.green : Colors.red),
              ),
              onTap: isValid
                  ? () {
                      // Trả cả mã chiết khấu và giá trị về cho widget
                      Navigator.pop(context, {
                        'maChietKhau': discount.maChietKhau,
                        'giaTri': discount.giaTriChietKhau,
                      });
                    }
                  : null,
            );
          },
        );
      },
    );
  }
}
