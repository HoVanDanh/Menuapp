import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:menuapp/Model/ban.dart';
import 'package:menuapp/Model/chitietdatban.dart';
import 'package:menuapp/Model/monAns.dart';
import 'package:http/http.dart' as http;
import 'package:menuapp/controller/ban_controller.dart';
import 'package:menuapp/controller/chitietdatban_controller.dart';

class OrderController extends ChangeNotifier {
  List<MonAn> monAnList = [];
  List<int> quantities = [];
  List<String> dsloaiMonAn = [
    "Tất cả",
    "Món chính",
    "Thức uống",
    "Món khai vị"
  ]; 
  List<MonAn> filteredMonAnList = [];

  Future<void> DsMonAn() async {
    try {
      final response =
          await http.get(Uri.parse('http://10.0.2.2:7001/api/MonAns/dsMonan'));
      if (response.statusCode == 200) {
        List<dynamic> data = json.decode(response.body);
        monAnList = data.map((json) => MonAn.fromJson(json)).toList();
        filteredMonAnList = List.from(monAnList);
        quantities = List.filled(monAnList.length, 0);
      } else {
        throw Exception('Failed to load monAn');
      }
    } catch (error) {
      print('Error fetching monAn: $error');
    }
  }

  void filterMonAnByLoai(String? loaiMonAn) {
    if (loaiMonAn == null || loaiMonAn == "Tất cả") {
      // Nếu không chọn loại món ăn, hiển thị tất cả món ăn
      filteredMonAnList = monAnList;
    } else {
      filteredMonAnList =
          monAnList.where((monAn) => monAn.loaiMonAn == loaiMonAn).toList();     
    }
    notifyListeners();
  }
    void resetOrder() {
    quantities = List.filled(quantities.length, 0);
  }
  // Hàm chốt món ăn
  void chotMonAn(Ban ban, BuildContext context) {
    final selectedMonAnList = filteredMonAnList
        .where((monAn) => quantities[filteredMonAnList.indexOf(monAn)] > 0)
        .toList();

    // Tạo danh sách các món ăn đã chọn cùng số lượng
    final monandachon = selectedMonAnList.map((monAn) {
      final quantity = quantities[filteredMonAnList.indexOf(monAn)];
      return {'monAn': monAn, 'quantity': quantity};
    }).toList();
    if (monandachon.length > 0) {
      ban.trangThai = true;
      chitietdatbanController.taoChiTietDatBan(
          monandachon, ban.banId);
      // cập nhập trạng thái bàn xuống db
      banController.updateTrangThaiBan(ban.banId, ban.trangThai, context);
      resetOrder();
    } else {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Chưa có món ăn nào được chọn!'),
          duration: Duration(seconds: 2),
        ),
      );
    }
  }
}

final OrderController _orderController = OrderController();
