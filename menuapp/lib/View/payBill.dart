import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:menuapp/Model/ban.dart';
import 'package:menuapp/Model/chitietdatban.dart';
import 'package:menuapp/Model/khachHang.dart';
import 'package:menuapp/View/widget/disscount_buttonsheet.dart';
import 'package:menuapp/View/widget/vatWidget.dart';
import 'package:menuapp/controller/khachhang_controller.dart';
import 'package:menuapp/controller/payBill_controller.dart';

class Paybill extends StatefulWidget {
  final List<ChiTietDatBan> selectedItems;
  final Ban ban;

  const Paybill({super.key, required this.selectedItems, required this.ban});

  @override
  _PaybillState createState() => _PaybillState();
}

class _PaybillState extends State<Paybill> {
  final PaybillController _controller = PaybillController();
  List<KhachHang> khachHangs = [];
  List<KhachHang> locKH = [];
  bool hienthiKH = false;
  String discountCode = '';
  @override
  void initState() {
    super.initState();
    _controller.initialize(widget.selectedItems, widget.ban);
    setState(() {
      _controller.TongslMon();
      _controller.tongtien();
      _controller.initialize(widget.selectedItems, widget.ban);
    });
    loaddsKH();
  }

// lấy ds khách hàng   từ api
  void loaddsKH() async {
    await khachhangController.dsKhachHang();
    setState(() {
      khachHangs = khachhangController.khachHangs
          .map((data) => KhachHang.fromJson(data))
          .toList();
      locKH = khachHangs;
    });
  }

  void fillterKh(String std) {
    if (std.isEmpty) {
      setState(() {
        locKH = khachHangs;
        hienthiKH = false;
      });
    } else {
      setState(() {
        locKH = khachHangs
            .where((khach) =>
                khach.soDienThoai.toLowerCase().contains(std.toLowerCase()) ||
                khach.tenKhachHang.toLowerCase().contains(std.toLowerCase()))
            .toList();
        hienthiKH = true;
        if (locKH.isEmpty) {
          setState(() {
            hienthiKH = false;
          });
        }
      });
    }
  }

  void _selectCustomer(KhachHang customer) {
    _controller.hoVaTenController.text = customer.tenKhachHang;
    _controller.sDTController.text = customer.soDienThoai;
    _controller.diaChiController.text = customer.diaChi;
    setState(() {
      hienthiKH = false;
    });
  }

  int tinhdiem(int tongtien) {
    return (tongtien / 100000).floor();
  }

  void xacnhanthanhtoan(BuildContext context) async {
    // Hiển thị hộp thoại xác nhận thanh toán
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text('Xác nhận thanh toán'),
          content: Text('Bạn có chắc chắn muốn thanh toán không?'),
          actions: <Widget>[
            TextButton(
              child: Text('Hủy'),
              onPressed: () {
                // Đóng dialog nếu người dùng chọn hủy
                Navigator.of(context).pop();
              },
            ),
            TextButton(
                child: Text('Xác nhận'),
                onPressed: () async {
                  _controller.taohoadon(context, discountCode);
                }),
          ],
        );
      },
    );
  }

  void _onDiscountCodeChanged(String code) {
    setState(() {
      discountCode = code;
    });
  }

  @override
  Widget build(BuildContext context) {
    int tt = _controller.tongtien();
    int diem = tinhdiem(tt);
    return Scaffold(
      appBar: AppBar(
        title: const Text('Thanh Toán'),
      ),
      body: SingleChildScrollView(
        child: Container(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              // Thông tin khách hàng
              const Text(
                'Thông tin khách hàng:',
                style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
              ),
              const SizedBox(height: 10),
              TextField(
                controller: _controller.hoVaTenController,
                decoration: const InputDecoration(
                  labelText: 'Họ và tên',
                  border: OutlineInputBorder(),
                ),
                onChanged: fillterKh,
              ),
              if (hienthiKH == true)
                Container(
                  padding: const EdgeInsets.only(top: 8.0),
                  height: 200,
                  child: ListView.builder(
                    itemCount: locKH.length,
                    itemBuilder: (context, index) {
                      return ListTile(
                        title: Text(locKH[index].tenKhachHang),
                        subtitle: Text(locKH[index].soDienThoai),
                        onTap: () => _selectCustomer(locKH[index]),
                      );
                    },
                  ),
                ),
              const SizedBox(height: 10),
              TextField(
                controller: _controller.sDTController,
                keyboardType: TextInputType.number,
                decoration: const InputDecoration(
                  labelText: 'Số điện thoại',
                  border: OutlineInputBorder(),
                ),
                onChanged: fillterKh,
              ),
              const SizedBox(height: 10),
              TextField(
                controller: _controller.diaChiController,
                decoration: const InputDecoration(
                  labelText: 'Địa chỉ',
                  border: OutlineInputBorder(),
                ),
              ),
              const SizedBox(height: 20),
              // chiec khau
              DiscountInputWidget(
                chietKhauController: _controller.chietKhauController,
                onDiscountCodeChanged: _onDiscountCodeChanged,
              ),
              if (discountCode.isNotEmpty)
                Text(
                  'Mã chiết khấu đã nhập: $discountCode',
                  style: const TextStyle(fontSize: 16, color: Colors.green),
                ),
              const SizedBox(height: 20),
              const SizedBox(height: 20),
              Text(
                'Tên bàn: ${widget.ban.tenBan}',
                style:
                    const TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
              ),

              // Hiển thị danh sách món ăn đã chọn
              const Text(
                'Danh sách món ăn đã chọn:',
                style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
              ),
              const SizedBox(height: 10),
              for (var item
                  in widget.selectedItems) // Use the selected items passed in
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Expanded(
                      child: Text(
                        '${item.tenMonAn} - ${item.gia} - sl: ${item.soLuong}',
                        style: const TextStyle(fontSize: 15),
                        overflow: TextOverflow.ellipsis,
                      ),
                    ),
                    Text(
                      '${item.gia * item.soLuong} ₫',
                      style: const TextStyle(fontSize: 15, color: Colors.red),
                    ),
                  ],
                ),
              const SizedBox(height: 20),
              // Hiển thị tổng số lượng món ăn và tổng tiền
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(
                    'Số lượng món ăn: ${_controller.TongslMon()}', // Calculate total based on selectedItems
                    style: const TextStyle(fontSize: 16, color: Colors.red),
                  ),
                  Text(
                    'Tổng: ${_controller.tongtien()} ₫', // Calculate total price based on selectedItems
                    style: const TextStyle(fontSize: 16, color: Colors.red),
                  ),
                ],
              ),
              const SizedBox(height: 20),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  const Text(
                    'Điểm tích lũy:',
                    style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                  ),
                  Text(
                    '$diem điểm',
                    style: const TextStyle(fontSize: 16, color: Colors.green),
                  ),
                ],
              ),
              const SizedBox(height: 20),
              // Chọn hình thức thanh toán
              const Text(
                'Hãy chọn hình thức thanh toán:',
                style: TextStyle(fontSize: 16, color: Colors.green),
              ),
              DropdownButton<String>(
                value: _controller.selectedPaymentMethod,
                hint: const Text('Chọn hình thức'),
                items: _controller.paymentMethods.map((String value) {
                  return DropdownMenuItem<String>(
                    value: value,
                    child: Text(value),
                  );
                }).toList(),
                onChanged: (String? newValue) {
                  setState(() {
                    _controller.selectedPaymentMethod = newValue;
                  });
                },
                underline: Container(),
                isExpanded: true,
                icon: const Icon(Icons.arrow_drop_down),
                iconEnabledColor: Colors.black,
                dropdownColor: Colors.white,
              ),
              const SizedBox(height: 20),

              // Nút thanh toán
              SizedBox(
                width: double.infinity,
                child: ElevatedButton(
                  onPressed: () {
                    xacnhanthanhtoan(context);
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.blue,
                    foregroundColor: Colors.white,
                  ),
                  child: const Text('Thanh Toán'),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
