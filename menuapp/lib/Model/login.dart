class Login {
  String tenDangNhap;
  String matKhau;

  Login({required this.tenDangNhap, required this.matKhau});

  Map<String, dynamic> toJson() {
    return {
      'tenDangNhap': tenDangNhap,
      'matKhau': matKhau,
    };
  }
}