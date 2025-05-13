import 'package:flutter/material.dart';
import 'package:menuapp/Model/account.dart';

class accountnprovider with ChangeNotifier {
  Account? acc;

  // Getter để lấy thông tin người dùng
  Account? get user => acc;

  // Hàm lưu thông tin người dùng vào provider
  void setUser(Account user) {
    acc = user;
    notifyListeners();
  }

  // Hàm đăng xuất (xóa thông tin người dùng)
  void logoutUser() {
    acc = null;
    notifyListeners();
  }
}