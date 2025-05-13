import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:menuapp/Model/ban.dart';
import 'package:menuapp/Model/chitietdatban.dart';
import 'package:menuapp/Model/hoaDon.dart';
import 'package:menuapp/Model/khachHang.dart';
import 'package:menuapp/Model/chitiethoadon.dart';
import 'package:menuapp/Model/taoHoaDon.dart';
import 'package:menuapp/View/xuat_pdf.dart';
import 'package:menuapp/controller/ban_controller.dart';
import 'package:menuapp/controller/chietkhau_controller.dart';
import 'package:menuapp/controller/chitietdatban_controller.dart';
import 'package:menuapp/controller/hoadon_controller.dart';
import 'package:provider/provider.dart';

class PaybillController {
  final TextEditingController hoVaTenController = TextEditingController();
  final TextEditingController sDTController = TextEditingController();
  final TextEditingController diaChiController = TextEditingController();
  final TextEditingController chietKhauController = TextEditingController();
  String? selectedPaymentMethod;
  final List<String> paymentMethods = ["Tiền mặt"];

  List<ChiTietDatBan> selectedItems = [];
  late Ban ban;

  void initialize(final List<ChiTietDatBan> items, Ban table) {
    selectedItems = items;
    ban = table;
  }

  int TongslMon() {
    return selectedItems.fold<int>(0, (sum, item) => sum + item.soLuong as int);
  }

 int tongtien() {
  // Tính tổng tiền trước khi áp dụng chiết khấu
  int totalAmount = selectedItems.fold<int>(
    0,
    (sum, item) => sum + (item.gia * item.soLuong),
  );

  // Lấy giá trị chiết khấu từ TextField
  String discountInput = chietKhauController.text.trim();
  int discount = 0;

  if (discountInput.isNotEmpty) {
    try {
      discount = int.parse(discountInput);
      if (discount < 0 ) {
        throw FormatException('Chiết khấu phải từ 0 đến 100');
      }
    } catch (e) {
      discount = 0; 
    }
  }

  // Tính tổng tiền sau khi áp dụng chiết khấu
  int finalAmount = discount > 0
      ? totalAmount - discount
      : totalAmount;  

  return finalAmount;
}

  Future<void> taohoadon(BuildContext context, String code) async {
    if (selectedItems.isEmpty ||
        !selectedItems.any((item) => item.soLuong > 0)) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
            content: Text('Vui lòng chọn món ăn trước khi thanh toán!')),
      );
      return;
    }
    if (selectedPaymentMethod == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Vui lòng chon phuong thuc thanh toan')),
      );
      return;
    }

    final khachHang = KhachHang(
      khachHangId: 0,
      tenKhachHang: hoVaTenController.text,
      soDienThoai: sDTController.text,
      diaChi: diaChiController.text,
    );

    final chiTietHoaDons = selectedItems.map((item) {
      return ChiTietHoaDonDTO(
        chiTietHoaDonId: 0,
        hoaDonId: 0,
        monAnId: item.monAnId,
        soLuong: item.soLuong,
        gia: item.gia,
      );
    }).toList();

    final hoaDon = HoaDon(
      hoaDonId: 0,
      khachHangId: 0,
      banId: ban.banId,
      ngayTao: DateTime.now(),
      tongTien: tongtien(),
      chiTietHoaDons: chiTietHoaDons,
    );

    final response = await http.post(
      Uri.parse('http://10.0.2.2:7001/api/HoaDons/TaoHoaDon'),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body:
          jsonEncode(TaoHoaDon(hoaDon: hoaDon, khachHang: khachHang).toJson()),
    );
    ban.trangThai = false;
    if (response.statusCode == 201) {
      if(code != null){
        await chietkhau.giamSoLuong(code);
      }
        
      var jsonResponse = jsonDecode(response.body);
      int hoaDonId = jsonResponse['hoaDonId'];
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Thanh toán thành công!')),
      );
      Navigator.push(
        context,
        MaterialPageRoute(
            builder: (context) => HoaDonScreen(
                  hoaDonId: hoaDonId,
                  chieckhau: chietKhauController.text,
                )),
      );
      chitietdatbanController.xoaMonAnCuaBan(ban.banId);
      banController.updateTrangThaiBan(ban.banId, ban.trangThai, context);
    } else {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Thanh toán thất bại!')),
      );
    }
  }
  
  // xac nhan thanh toan 
   Future<void> xacNhanThanhToanVaXoaMonAn(int hoaDonId, BuildContext context) async {
  final thanhToanResponse = await hoadonController.xacNhanThanhToan(hoaDonId);
  if (thanhToanResponse) {
    ScaffoldMessenger.of(context).showSnackBar(
      const SnackBar(content: Text('Thanh toán và xóa món ăn thành công!')),
    );
      chitietdatbanController.xoaMonAnCuaBan(ban.banId);
      banController.updateTrangThaiBan(ban.banId, ban.trangThai, context);
  } else { 
    ScaffoldMessenger.of(context).showSnackBar(
      const SnackBar(content: Text('Xác nhận thanh toán thất bại!')),
    );
  }
}
}
PaybillController paybillController = PaybillController();
