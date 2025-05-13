// nhan_vien.dart

class NhanVien {
  int nhanVienId;
  String tenNhanVien;
  String soDienThoai;
  String diaChi;
  int loaiNhanVienId;

  NhanVien({
    required this.nhanVienId,
    required this.tenNhanVien,
    required this.soDienThoai,
    required this.diaChi,
    required this.loaiNhanVienId,
  });

  // Chuyển JSON thành đối tượng NhanVien
  factory NhanVien.fromJson(Map<String, dynamic> json) {
    return NhanVien(
      nhanVienId: json['nhanVienId'],
      tenNhanVien: json['tenNhanVien'],
      soDienThoai: json['soDienThoai'],
      diaChi: json['diaChi'],
      loaiNhanVienId: json['loaiNhanVienId'],
    );
  }
}
