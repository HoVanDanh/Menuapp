class MonAn {
  int monAnId;
  String tenMon;
  String loaiMonAn; 
  int gia;
  String img;

  MonAn({
    required this.monAnId,
    required this.tenMon,
    required this.loaiMonAn, 
    required this.gia,
    required this.img,
  });

  // Phương thức để tạo đối tượng từ JSON
  factory MonAn.fromJson(Map<String, dynamic> json) {
    return MonAn(
      monAnId: json['monAnId'],
      tenMon: json['tenMon'],
      loaiMonAn: json['loaiMonAn'],  // Không chuyển đổi, lấy trực tiếp
      gia: json['gia'],
      img: json['img'],
    );
  }

  // Phương thức để chuyển đối tượng thành JSON
  Map<String, dynamic> toJson() {
    return {
      'monAnId': monAnId,
      'tenMon': tenMon,
      'loaiMonAn': loaiMonAn,  // Giữ nguyên loaiMonAn
      'gia': gia,
      'img': img,
    };
  }
  
}

