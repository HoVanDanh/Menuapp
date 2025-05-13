import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:menuapp/Model/ban.dart';

class BanController {
   List<Ban> tables = [];
  // Cập nhật trạng thái bàn trong cơ sở dữ liệu
  Future<void> updateTrangThaiBan(int banId, bool trangThai, BuildContext context) async {
    try {
      final response = await http.post(
        Uri.parse('http://10.0.2.2:7001/api/Bans/TrangThaiban'),
        headers: <String, String>{
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode({
          "banId": banId,
          "trangThai": trangThai,
        }),
      );
      if (response.statusCode == 200) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Cập nhật trạng thái bàn thành công!')),
        );
      } else {
        // Xử lý khi cập nhật không thành công
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Cập nhật trạng thái bàn thất bại!')),
        );
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Có lỗi xảy ra, vui lòng thử lại!')),
      );
    }
  }
Future<List<Ban>> dsban() async {
    try {
      final response = await http.get(Uri.parse('http://10.0.2.2:7001/api/Bans/dsBans'));
      if (response.statusCode == 200) {
        List<dynamic> data = json.decode(response.body);
        return data.map((item) => Ban.fromJson(item)).toList();
      } else {
        throw Exception('Failed to load tables: ${response.statusCode}');
      }
    } catch (e) {
      print('Error: $e');
      return [];
    }
  }
}
  final BanController banController = BanController();