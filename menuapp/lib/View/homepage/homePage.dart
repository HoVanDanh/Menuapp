import 'dart:async';

import 'package:flutter/material.dart';
import 'package:menuapp/Model/ban.dart';
import 'package:menuapp/View/Orderpage/listOrders.dart';
import 'package:menuapp/View/loginPage.dart';
import 'package:menuapp/View/widget/tablecar.dart';
import 'package:menuapp/controller/ban_controller.dart';
import 'package:menuapp/provider/accountProvider.dart';
import 'package:provider/provider.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  List<Ban> tables = [];
    Timer? _timer;
  @override
  void initState() {
    super.initState();
    // Call the function to fetch data after the frame is built
    WidgetsBinding.instance.addPostFrameCallback((_) {
      dsban();
          _timer = Timer.periodic(const Duration(seconds: 30), (timer) {
      dsban();
    });
    });
  }

  Future<void> dsban() async {
    if (!mounted) return;
    // goi ham lay ds ban
    tables = await banController.dsban();
    setState(() {
    banController.dsban();
    });
  }
void showLogoutDialog() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Xác nhận đăng xuất'),
        content: const Text('Bạn có chắc chắn muốn đăng xuất không?'),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(context).pop(),
            child: const Text('Hủy'),
          ),
          TextButton(
            onPressed: () {
              Provider.of<accountnprovider>(context, listen: false).logoutUser();
              Navigator.pushAndRemoveUntil(
                context,
                MaterialPageRoute(builder: (context) => const LoginPage()),
                (route) => false,
              );
            },
            child: const Text('Đăng xuất'),
          ),
        ],
      ),
    );
  }
  @override
  Widget build(BuildContext context) {
    final acc = Provider.of<accountnprovider>(context).user;
    return Scaffold(
      backgroundColor: const Color(0xFFF0F0F0),
      appBar: AppBar(
        backgroundColor: const Color(0xFF1E88E5),
        title: const Text('Home Page'),
      ),
      drawer: Drawer(
        width: 350,
        child: ListView(
          padding: EdgeInsets.zero,
          children: [
            if (acc != null)
              UserAccountsDrawerHeader(
                accountName: Text("xin chao " + acc.nhanVien.tenNhanVien),
                accountEmail: Text(acc.nhanVien.soDienThoai),
              ),
            ListTile(
              leading: const Icon(Icons.logout),
              title: const Text('Đăng xuất'),
              onTap:showLogoutDialog
            ),
          ],
        ),
      ),
      body:
          // Nội dung chính
          SingleChildScrollView(
        child: Column(
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              crossAxisAlignment: CrossAxisAlignment.start,
              mainAxisSize: MainAxisSize.min,
              children: [
                Container(
                  alignment: Alignment.center,
                  margin: const EdgeInsets.fromLTRB(10, 5, 10, 5),
                  width: MediaQuery.of(context).size.width * 0.9,
                  height: MediaQuery.of(context).size.height * 0.22,
                  decoration: BoxDecoration(
                    color: Colors.orange[300]?.withOpacity(0.8),
                    border: Border.all(
                      color: Colors.black,
                      width: 1,
                    ),
                    borderRadius: BorderRadius.circular(20),
                    boxShadow: const [
                      BoxShadow(
                        color: Colors.black26,
                        blurRadius: 5,
                        offset: Offset(2, 2), // Độ lệch của bóng
                      ),
                    ],
                  ),
                  child: Column(
                    children: [
                      const SizedBox(height: 10),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          ClipOval(
                            child: Image.asset(
                              'assets/logo.jpg', // Đường dẫn tới ảnh logo
                              width: 100,
                              height: 100,
                              fit: BoxFit.cover,
                            ),
                          ),
                        ],
                      ),
                      const SizedBox(height: 5),
                      const Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Text(
                            "Thế giới STEAK",
                            style: TextStyle(fontSize: 20, color: Colors.red),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
              ],
            ),
            const SizedBox(height: 10),
            Padding(
              padding: const EdgeInsets.all(16.0),
              child: Column(
                children: [
                  SizedBox(
                    height: MediaQuery.of(context).size.height * 0.5,
                    child: GridView.builder(
                      physics: const NeverScrollableScrollPhysics(),
                      shrinkWrap: true,
                      itemCount: tables.length,
                      gridDelegate:
                          const SliverGridDelegateWithFixedCrossAxisCount(
                        crossAxisCount: 3,
                        crossAxisSpacing: 10,
                        mainAxisSpacing: 10,
                      ),
                      itemBuilder: (context, index) {
                        return TableCard(
                          ban: tables[index],
                          onTap: () {
                            Navigator.push(
                              context,
                              MaterialPageRoute(
                                builder: (context) =>
                                    ListOrders(ban: tables[index]),
                              ),
                            );
                          },
                        );
                      },
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
