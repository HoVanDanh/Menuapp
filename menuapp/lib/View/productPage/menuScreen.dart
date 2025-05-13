
import 'package:flutter/material.dart';
import 'package:menuapp/Model/monAns.dart';
import 'package:menuapp/View/widget/card_product.dart';
import 'package:menuapp/controller/monan_controller.dart';

class MenuScreen extends StatefulWidget {
  const MenuScreen({super.key});

  @override
  State<MenuScreen> createState() => _MenuScreenState();
}

class _MenuScreenState extends State<MenuScreen> {
  final TextEditingController _timKiem = TextEditingController();
  final MonanController _monanController = MonanController();
  List<MonAn> filteredList = [];

  @override
  void initState() {
    super.initState();
    _fetchData();
    _timKiem.addListener(_onSearchChanged);
  }

  void _fetchData() async {
    await _monanController.DsMonAn();
    setState(() {
      filteredList =
          _monanController.monAnList; // Hiển thị tất cả món ăn ban đầu
    });
  }

  void _onSearchChanged() {
    final query = _timKiem.text.toLowerCase();
    setState(() {
      if (query.isEmpty) {
        filteredList = _monanController.monAnList;
      } else {
        filteredList = _monanController.monAnList.where((monAn) {
          return monAn.tenMon.toLowerCase().contains(query);
        }).toList();
      }
    });
  }

  @override
  void dispose() {
    _timKiem.removeListener(_onSearchChanged);
    _timKiem.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF0F0F0),
      appBar: AppBar(
        title: const Text('Danh sách món ăn'),
        backgroundColor: const Color(0xFF1E88E5),
        iconTheme: const IconThemeData(color: Colors.black),
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () {
            Navigator.pop(context);
          },
        ),
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: TextField(
              controller: _timKiem,
              decoration: const InputDecoration(
                hintText: 'Nhập tên, mã sản phẩm',
                border: OutlineInputBorder(),
                prefixIcon: Icon(Icons.search),
              ),
            ),
          ),
          Expanded(
            child: filteredList.isEmpty
                ? const Center(child: Text('Không tìm thấy món ăn nào.'))
                : GridView.builder(
                    gridDelegate:
                        const SliverGridDelegateWithFixedCrossAxisCount(
                      crossAxisCount: 2,
                      childAspectRatio: 0.7,
                      crossAxisSpacing: 10,
                      mainAxisSpacing: 10,
                    ),
                    padding: const EdgeInsets.all(8.0),
                    itemCount: filteredList.length,
                    itemBuilder: (context, index) {
                      return CardMonAn(
                        monAn: filteredList[index],
                      );
                    },
                  ),
          ),
        ],
      ),
    );
  }
}
