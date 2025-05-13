// account.dart
import 'package:menuapp/Model/nhanvien.dart';


class Account {
  int accountId;
  String tenDangNhap;
  String matKhau;
  int phanQuyen;
  NhanVien nhanVien;

  Account({
    required this.accountId,
    required this.tenDangNhap,
    required this.matKhau,
    required this.phanQuyen,
    required this.nhanVien,
  });

  // Chuyển JSON thành đối tượng Account
  factory Account.fromJson(Map<String, dynamic> json) {
    return Account(
      accountId: json['accountId'],
      tenDangNhap: json['tenDangNhap'],
      matKhau: json['matKhau'],
      phanQuyen: json['phanQuyen'],
      nhanVien: NhanVien.fromJson(json['nhanVien']),
    );
  }
}