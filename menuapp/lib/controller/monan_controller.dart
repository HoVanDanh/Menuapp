  import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:menuapp/Model/ban.dart';
import 'package:menuapp/Model/chitietdatban.dart';
import 'package:menuapp/Model/monAns.dart';
import 'package:http/http.dart' as http;
import 'package:menuapp/controller/ban_controller.dart';
import 'package:provider/provider.dart';

  
class  MonanController {
   List<MonAn> monAnList = [];
    // Hàm lấy danh sách món ăn từ API
  Future<void> DsMonAn() async {
    final url='http://10.0.2.2:7001/api/MonAns/dsMonan';
    try {
      final response = await http.get(Uri.parse(url));
      if (response.statusCode == 200) {
        List<dynamic> data = json.decode(response.body);
        monAnList = data.map((json) => MonAn.fromJson(json)).toList();
      } else {
        throw Exception('Failed to load monAn');
      }
    } catch (error) {
      print('Error fetching monAn: $error');
    }
  }
  Future<int> getSoLuongPhanConLai(int id) async {
    final url = Uri.parse('http://10.0.2.2:7001/api/MonAns/soLuongPhanConLai/$id');
    
    try {
      final response = await http.get(url);

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        return data;
      } else {
        return 1000;
      }
    } catch (e) {
      return 0;
    }
}
}
final MonanController monanController = MonanController();