class KhachHang {
  int khachHangId;
  String tenKhachHang;
  String soDienThoai;
  String diaChi;

  KhachHang({
    required this.khachHangId,
    required this.tenKhachHang,
    required this.soDienThoai,
    required this.diaChi,
  });

  factory KhachHang.fromJson(Map<String, dynamic> json) {
    return KhachHang(
      khachHangId: json['khachHangId'],
      tenKhachHang: json['tenKhachHang'],
      soDienThoai: json['soDienThoai'],
      diaChi: json['diaChi'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'khachHangId': khachHangId,
      'tenKhachHang': tenKhachHang,
      'soDienThoai': soDienThoai,
      'diaChi': diaChi,
    };
  }
}