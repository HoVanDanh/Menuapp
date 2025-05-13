import 'package:menuapp/Model/chitiethoadon.dart';

class HoaDon {
  int hoaDonId;
  int khachHangId;
  int banId;
  DateTime ngayTao;
  int tongTien;
  List<ChiTietHoaDonDTO> chiTietHoaDons;

  HoaDon({
    required this.hoaDonId,
    required this.khachHangId,
    required this.banId,
    required this.ngayTao,
    required this.tongTien,
    required this.chiTietHoaDons,
  });

  factory HoaDon.fromJson(Map<String, dynamic> json) {
    return HoaDon(
      hoaDonId: json['hoaDonId'],
      khachHangId: json['khachHangId'],
      banId: json['banId'],
      ngayTao: DateTime.parse(json['ngayTao']),
      tongTien: json['tongTien'].toDouble(),
      chiTietHoaDons: List<ChiTietHoaDonDTO>.from(
          json['chiTietHoaDons'].map((x) => ChiTietHoaDonDTO.fromJson(x))),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'hoaDonId': hoaDonId,
      'khachHangId': khachHangId,
      'banId': banId,
      'ngayTao': ngayTao.toIso8601String(),
      'tongTien': tongTien,
      'chiTietHoaDons': List<dynamic>.from(chiTietHoaDons.map((x) => x.toJson())),
    };
  }
}

