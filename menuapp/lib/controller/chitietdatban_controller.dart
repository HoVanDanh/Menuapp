import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:menuapp/Model/chitietdatban.dart';

class ChitietdatbanController {
  // lay chi tiet dat ban
  Future<List<ChiTietDatBan>> getChiTietDatBan(int banId) async {
    final apiUrl ='http://10.0.2.2:7001/api/ChiTietDatBans/laychitietmonan/$banId';

    try {
      final response = await http.get(Uri.parse(apiUrl));
      if (response.statusCode == 200) {
        List<dynamic> data = json.decode(response.body);
        return data.map((json) => ChiTietDatBan.fromJson(json)).toList();
      } else if (response.statusCode == 404) {
        return [];
      } else {
        return [];
      }
    } catch (error) {
      print("Error fetching data: $error");
      return [];
    }
  }

// tao chi tiet dat ban
  Future<void> taoChiTietDatBan(
      List<Map<String, dynamic>> selectedItems, int banId) async {
    final apiUrl = 'http://10.0.2.2:7001/api/ChiTietDatBans/taochitiecdatban';
    // Chuyển đổi danh sách món ăn đã chọn thành danh sách chi tiết đặt bàn
    for (var item in selectedItems) {
      final chiTietDatBan = {
        'banId': banId,
        'monAnId': item['monAn'].monAnId,
        'soLuong': item['quantity'],
        'gia': item['monAn'].gia
      };

      try {
        final response = await http.post(
          Uri.parse(apiUrl),
          headers: {"Content-Type": "application/json"},
          body: json.encode(chiTietDatBan),
        );
        if (response.statusCode == 200) {
          throw Exception('tạo chi tiết đạt bàn thành công');
        }
      } catch (error) {
        print('Error creating chi tiết đặt bàn: $error');
      }
    }
  }

  /// xoa cac mon an cua ban khi thanh toans
  Future<void> xoaMonAnCuaBan(int banId) async {
    final response = await http.delete(
      Uri.parse(
          'http://10.0.2.2:7001/api/ChiTietDatBans/xoamonancuaban/$banId'),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
      },
    );

    if (response.statusCode == 204) {
    } else {}
  }

  Future<void> xoa1monancuaban(int id ) async {
    final url = Uri.parse(
        'http://10.0.2.2:7001/api/ChiTietDatBans/xoa1monancuaban/$id');

    try {
      final response = await http.delete(url);

      if (response.statusCode == 204) {
        print("Xóa món ăn thành công");
      } else {
        print("Lỗi khi xóa món ăn: ${response.statusCode}");
      }
    } catch (e) {
      print("Lỗi kết nối tới server: $e");
    }
  }
//   Future<void> chuyenMonAn(int banIdNguon, int banIdDich) async {
//   final url = 'http://localhost:7001/api/ChiTietDatBans/chuyenmonan/$banIdNguon/$banIdDich';
  
//   final response = await http.put(Uri.parse(url));
  
//   if (response.statusCode == 204) {
//     // Thành công, món ăn đã được chuyển sang bàn đích
//     print("Chuyển món ăn thành công!");
//   } else {
//     // Lỗi
//     print("Không thể chuyển món ăn. Mã lỗi: ${response.statusCode}");
//   }
// }
  Future<void> xacnhanramon(int id) async {
    final url = Uri.parse(
        'http://10.0.2.2:7001/api/ChiTietDatBans/xacnhanmonan/$id');

    try {
      final response = await http.post(url);
      print(response.body);
      if (response.statusCode == 200) {
        print("ra món ăn thành công");
      } else {
        print("Lỗi khi xóa món ăn: ${response.statusCode}");
      }
    } catch (e) {
      print("Lỗi kết nối tới server: $e");
    }
  }

}

final ChitietdatbanController chitietdatbanController =
    ChitietdatbanController();
 