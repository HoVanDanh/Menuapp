class ChietKhau {
  final int chietKhauId;
  final String maChietKhau;
  final int giaTriChietKhau; 
  final DateTime ngayBatDau;
  final DateTime ngayKetThuc;
  final int soLuong;

  ChietKhau({
    required this.chietKhauId,
    required this.maChietKhau,
    required this.giaTriChietKhau,
    required this.ngayBatDau,
    required this.ngayKetThuc,
    required this.soLuong,
  });

  // Tạo đối tượng từ JSON
  factory ChietKhau.fromJson(Map<String, dynamic> json) {
    return ChietKhau(
      chietKhauId: json['chietKhauId'],
      maChietKhau: json['maChietKhau'],
      giaTriChietKhau: json['giaTriChietKhau'],
      ngayBatDau: DateTime.parse(json['ngayBatDau']),
      ngayKetThuc: DateTime.parse(json['ngayKetThuc']),
      soLuong: json['soLuong'],
    );
  }

  // Chuyển đối tượng sang JSON
  Map<String, dynamic> toJson() {
    return {
      'chietKhauId': chietKhauId,
      'maChietKhau': maChietKhau,
      'giaTriChietKhau': giaTriChietKhau,
      'ngayBatDau': ngayBatDau.toIso8601String(),
      'ngayKetThuc': ngayKetThuc.toIso8601String(),
      'soLuong': soLuong,
    };
  }
}
