import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:menuapp/Model/khachHang.dart';

class KhachhangController {
   List<dynamic> khachHangs = []; 
    Future<void> dsKhachHang() async {
    final response = await http.get(Uri.parse('http://10.0.2.2:7001/api/KhachHangs/dskhachhang'));

    if (response.statusCode == 200) {
      khachHangs = json.decode(response.body);
    } else {
      throw Exception('Failed to load customers');
    }
  }
}
final KhachhangController khachhangController = KhachhangController();