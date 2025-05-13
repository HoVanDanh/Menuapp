class ChiTietHoaDonDTO {
  int chiTietHoaDonId;
  int hoaDonId;
  int monAnId;
  int soLuong;
  int gia;

  ChiTietHoaDonDTO({
    required this.chiTietHoaDonId,
    required this.hoaDonId,
    required this.monAnId,
    required this.soLuong,
    required this.gia,
  });

  factory ChiTietHoaDonDTO.fromJson(Map<String, dynamic> json) {
    return ChiTietHoaDonDTO(
      chiTietHoaDonId: json['chiTietHoaDonId'],
      hoaDonId: json['hoaDonId'],
      monAnId: json['monAnId'],
      soLuong: json['soLuong'],
      gia: json['gia'].toDouble(),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'chiTietHoaDonId': chiTietHoaDonId,
      'hoaDonId': hoaDonId,
      'monAnId': monAnId,
      'soLuong': soLuong,
      'gia': gia,
    };
  }
}