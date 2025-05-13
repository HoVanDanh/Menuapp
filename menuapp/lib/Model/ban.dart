import 'dart:convert';
import 'package:http/http.dart' as http;

// BansDTO model
class Ban {
  final int banId;
  final String tenBan;
   bool trangThai;
   //int thoiGianNgoi;
   List<Map<String, dynamic>> selectedItems; 

  Ban({required this.banId,
   required this.tenBan, 
   required this.trangThai,
   //required this.thoiGianNgoi,
   this.selectedItems = const [],
   });

  factory Ban.fromJson(Map<String, dynamic> json) {
    return Ban(
      banId: json['banId'],
      tenBan: json['tenBan'],
      trangThai: json['trangThai'],
     //thoiGianNgoi: json['thoiGianNgoi'],
    );
  }

  // Hàm lấy dữ liệu từ API
  static  Future<List<Ban>> dsban() async {
    try {
      final response = await http.get(Uri.parse('http://192.168.1.11:7001/Bans/dsBans'));

      // Kiểm tra mã trạng thái
      if (response.statusCode == 200) {
        List<dynamic> data = json.decode(response.body);
        // Chuyển đổi danh sách JSON thành danh sách đối tượng Ban
        return data.map((item) => Ban.fromJson(item)).toList();
      } else {
        throw Exception('Failed to load tables: ${response.statusCode}');
      }
    } catch (e) {
      print('Error occurred: $e');
      throw e; // Ném lại lỗi để xử lý bên ngoài nếu cần
    }
  }
}
