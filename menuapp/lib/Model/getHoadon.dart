class ChiTietHoaDon {
  final int chiTietHoaDonId;
  final int hoaDonId;
  final int monAnId;
  final String tenMon;
  final int soLuong;
  final int gia;

  ChiTietHoaDon({
    required this.chiTietHoaDonId,
    required this.hoaDonId,
    required this.monAnId,
    required this.tenMon,
    required this.soLuong,
    required this.gia,
  });

  factory ChiTietHoaDon.fromJson(Map<String, dynamic> json) {
    return ChiTietHoaDon(
      chiTietHoaDonId: json['chiTietHoaDonId'],
      hoaDonId: json['hoaDonId'],
      monAnId: json['monAnId'],
      tenMon: json['tenMon'],
      soLuong: json['soLuong'],
      gia: json['gia'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'chiTietHoaDonId': chiTietHoaDonId,
      'hoaDonId': hoaDonId,
      'monAnId': monAnId,
      'tenMon': tenMon,
      'soLuong': soLuong,
      'gia': gia,
    };
  }
}

class HoaDon {
  final int hoaDonId;
  final int khachHangId;
  final String tenKhachHanh;
  final int banId;
  final DateTime ngayTao;
  final int tongTien;
  final List<ChiTietHoaDon> chiTietHoaDons;

  HoaDon({
    required this.hoaDonId,
    required this.khachHangId,
    required this.tenKhachHanh,
    required this.banId,
    required this.ngayTao,
    required this.tongTien,
    required this.chiTietHoaDons,
  });

  factory HoaDon.fromJson(Map<String, dynamic> json) {
    var chiTietList = json['chiTietHoaDons'] as List;
    List<ChiTietHoaDon> chiTietHoaDonsList =
        chiTietList.map((item) => ChiTietHoaDon.fromJson(item)).toList();

    return HoaDon(
      hoaDonId: json['hoaDonId'],
      khachHangId: json['khachHangId'],
      tenKhachHanh: json['tenKhachHanh'],
      banId: json['banId'],
      ngayTao: DateTime.parse(json['ngayTao']),
      tongTien: json['tongTien'],
      chiTietHoaDons: chiTietHoaDonsList,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'hoaDonId': hoaDonId,
      'khachHangId': khachHangId,
      'tenKhachHanh': tenKhachHanh,
      'banId': banId,
      'ngayTao': ngayTao.toIso8601String(),
      'tongTien': tongTien,
      'chiTietHoaDons': chiTietHoaDons.map((item) => item.toJson()).toList(),
    };
  }
}
