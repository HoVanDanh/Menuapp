import 'package:menuapp/Model/hoaDon.dart';
import 'package:menuapp/Model/khachHang.dart';

class TaoHoaDon {
  HoaDon hoaDon;
  KhachHang khachHang;

  TaoHoaDon({
    required this.hoaDon,
    required this.khachHang,
  });

  factory TaoHoaDon.fromJson(Map<String, dynamic> json) {
    return TaoHoaDon(
      hoaDon: HoaDon.fromJson(json['hoaDon']),
      khachHang: KhachHang.fromJson(json['khachHang']),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'hoaDon': hoaDon.toJson(),
      'khachHang': khachHang.toJson(),
    };
  }
}