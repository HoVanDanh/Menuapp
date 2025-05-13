import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:menuapp/Model/ban.dart';
import 'package:menuapp/Model/getHoadon.dart';
class HoadonController {
  // laay thong tin hoa don 
  Future<HoaDon?> layhoadon(int hoaDonid) async {
    try {
      final response = await http.get(Uri.parse('http://10.0.2.2:7001/api/HoaDons/$hoaDonid'));
      if (response.statusCode == 200) {
        return HoaDon.fromJson(json.decode(response.body)); // Chuyển đổi JSON thành đối tượng HoaDon
      } else {
        throw Exception('Failed to load invoice');
      }
    } catch (error) {
      print('Error fetching invoice: $error');
      return null;
    }
  }
 /// huy thanh toan hoa don 
Future<void> huyHoaDon(int hoaDonId) async {
  try {
    final response = await http.delete(
      Uri.parse('http://10.0.2.2:7001/api/HoaDons/HuyHoaDon/$hoaDonId'),
    );

    if (response.statusCode == 200) {
      print('Hóa đơn đã được hủy');
    } else {
      print('Không thể hủy hóa đơn. Mã lỗi: ${response.statusCode}');
    }
  } catch (error) {
    print('Error canceling invoice: $error');
  }
}
Future<bool> xacNhanThanhToan(int hoaDonId) async {
    try {
      final response = await http.put(
        Uri.parse('http://10.0.2.2:7001/api/HoaDons/XacNhanThanhToan/$hoaDonId'),
      );

      if (response.statusCode == 200) {
        print('Hóa đơn đã được xác nhận thanh toán');
        return true;
      } else {
        print('Không thể xác nhận thanh toán. Mã lỗi: ${response.statusCode}');
        return false;
      }
    } catch (error) {
      print('Error confirming payment: $error');
      return false;
    }
  }
}


final HoadonController hoadonController = HoadonController();
