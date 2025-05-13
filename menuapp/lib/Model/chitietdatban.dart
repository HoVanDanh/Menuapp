class ChiTietDatBan {
  final int id;
  final int banId;
  final int monAnId;
  final String tenMonAn;
  final int soLuong;
  final int gia;
  final int timeconlai;
  final bool trangThai;
  

  ChiTietDatBan({
    required this.id,
    required this.banId,
    required this.monAnId,
    required this.tenMonAn,
    required this.soLuong,
    required this.gia,
    required this.timeconlai,
    required this.trangThai
  });

  // Phương thức để chuyển đổi từ JSON sang model
  factory ChiTietDatBan.fromJson(Map<String, dynamic> json) {
    return ChiTietDatBan(
      id: json['id'],
      banId: json['banId'],
      monAnId: json['monAnId'],
      tenMonAn: json['tenMonAn'],
      soLuong: json['soLuong'],
      gia: json['gia'],
      timeconlai: json['timeConLai'],
      trangThai: json['trangThai'],
    );
  }

  // Phương thức để chuyển đổi từ model sang JSON
  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'banId': banId,
      'monAnId': monAnId,
      'tenMonAn': tenMonAn,
      'soLuong': soLuong,
      'gia': gia,
    };
  }
}