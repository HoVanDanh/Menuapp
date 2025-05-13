import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:menuapp/Model/chietkhau.dart';

class ChietkhauController {
  final String baseUrl = "http://10.0.2.2:7001/api/ChietKhaus/dschietkhau";

  // Hàm lấy danh sách chiết khấu
  Future<List<ChietKhau>> fetchChietKhaus() async {
    try {
      final response = await http.get(Uri.parse(baseUrl));

      if (response.statusCode == 200) {
        List<dynamic> jsonData = jsonDecode(response.body);
        return jsonData.map((e) => ChietKhau.fromJson(e)).toList();
      } else {
        throw Exception("Failed to load chiết khấu: ${response.statusCode}");
      }
    } catch (e) {
      throw Exception("Error fetching chiết khấu: $e");
    }
  }
     Future<void> giamSoLuong(String maChietKhau) async {
    final apiUrl = 'http://10.0.2.2:7001/api/ChietKhaus/giamsoLuong/$maChietKhau';

    try {
      final response = await http.post(Uri.parse(apiUrl));

      if (response.statusCode == 200) {
        // Handle success
        print('thanh cong $maChietKhau');
      } else {
        // Handle failure
        print('ko co chiet khau');
      }
    } catch (error) {
      print('doi');
    }
  }
}
final ChietkhauController chietkhau = ChietkhauController ();