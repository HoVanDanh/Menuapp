import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:menuapp/Model/account.dart';
import 'package:menuapp/provider/accountProvider.dart';
import 'package:provider/provider.dart';

class LoginController {
  Future<bool> login(String username, String password, BuildContext context) async {
    final url = Uri.parse('http://10.0.2.2:7001/api/Accounts/login');

    final body = jsonEncode({
      'tenDangNhap': username,
      'matKhau': password,
    });

    try {
      final response = await http.post(
        url,
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: body,
      );

      // Kiểm tra trạng thái phản hồi
      if (response.statusCode == 200) {
        final rp= json.decode(response.body);
        Account acc= Account.fromJson(rp);
        Provider.of<accountnprovider>(context, listen: false).setUser(acc);
        return true;
      } else {
        return false;
      }
    } catch (error) {
      return false;
    }
  }
}
final LoginController _loginController = LoginController();
