import 'dart:async';

import 'package:flutter/material.dart';
import 'package:menuapp/Model/ban.dart';
import 'package:menuapp/Model/monAns.dart';
import 'package:menuapp/Model/chitietdatban.dart';
import 'package:menuapp/View/widget/order_card.dart';
import 'package:menuapp/View/widget/order_footer.dart';
import 'package:menuapp/View/widget/order_header.dart';
import 'package:menuapp/View/widget/product_section.dart';
import 'package:menuapp/controller/chitietdatban_controller.dart';
import 'package:menuapp/controller/order_controller.dart';

class ListOrders extends StatefulWidget {
  const ListOrders({super.key, required this.ban});
  final Ban ban;

  @override
  State<ListOrders> createState() => _ListOrdersState();
}

class _ListOrdersState extends State<ListOrders> {
  final OrderController _orderController = OrderController();
  bool showSelectedItems = false;
  bool showAllItems = true;
  Future<List<ChiTietDatBan>>? selectedItems;
  List<MonAn> filteredMonAnList = [];
  final TextEditingController _searchController = TextEditingController();
  bool isSearching = false;
  String? selectedLoaiMonAn;
  Timer? _timer;

  @override
  void initState() {
    super.initState();
    _fetchData();
    monandacgon();
    _timer = Timer.periodic(Duration(minutes: 1), (timer) {
      monandacgon();
    });

    _searchController.addListener(_onSearchChanged);
  }

  // lấy danh sách món ăn và hiển thị danh sách món
  void _fetchData() async {
    await _orderController.DsMonAn();
    setState(() {
      filteredMonAnList = _orderController.monAnList;
    });
  }

  // lấy danh sach món ăn từ chi tiết đặt bàn
  void monandacgon() async {
    selectedItems = chitietdatbanController.getChiTietDatBan(widget.ban.banId);
  }

  void goimon() async {}
  void _onSearchChanged() {
    final query = _searchController.text.toLowerCase();
    setState(() {
      isSearching = query.isNotEmpty;
      if (query.isEmpty) {
        filteredMonAnList = _orderController.monAnList;
      } else {
        filteredMonAnList = _orderController.monAnList.where((monAn) {
          return monAn.tenMon.toLowerCase().contains(query);
        }).toList();
      }
    });
  }

// hàm lọc theo loại món ăn
  void _onLoaiMonAnChanged(String? newLoaiMonAn) {
    setState(() {
      selectedLoaiMonAn = newLoaiMonAn;
      _orderController.filterMonAnByLoai(newLoaiMonAn);
      filteredMonAnList = _orderController.filteredMonAnList;
    });
  }

  void _toggleAllItems() {
    setState(() {
      showAllItems = true;
      showSelectedItems = false;
      selectedItems =
          chitietdatbanController.getChiTietDatBan(widget.ban.banId);
    });
  }

  void _toggleSelectedItems() {
    setState(() {
      showSelectedItems = true;
      showAllItems = false;
      selectedItems =
          chitietdatbanController.getChiTietDatBan(widget.ban.banId);
    });
  }

  void xoa1monChiTietDatBan(int id) async {
    await chitietdatbanController.xoa1monancuaban(id);
    setState(() {
      selectedItems =
          chitietdatbanController.getChiTietDatBan(widget.ban.banId);
    });
  }

  void xacnhanramon(int idmon) async {
    await chitietdatbanController.xacnhanramon(idmon);
    setState(() {
      selectedItems =
          chitietdatbanController.getChiTietDatBan(widget.ban.banId);
    });
  }

  @override
  void dispose() {
    _searchController.removeListener(_onSearchChanged);
    _searchController.dispose();
    _timer?.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF0F0F0),
      appBar: AppBar(
        title: const Text('Quản Lý Món Ăn'),
        backgroundColor: const Color(0xFF1E88E5),
        leading: IconButton(
          icon: const Icon(Icons.arrow_back),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: Column(
        children: [
          const OrderHeader(),
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: Row(
              children: [
                // TextField
                Expanded(
                  child: TextField(
                    controller: _searchController,
                    decoration: const InputDecoration(
                      hintText: 'Tìm kiếm món ăn...',
                      border: OutlineInputBorder(),
                      prefixIcon: Icon(Icons.search),
                    ),
                  ),
                ),
                const SizedBox(width: 10),
                // DropdownButton
                DropdownButton<String>(
                  value: selectedLoaiMonAn,
                  hint: const Text(' loại món'),
                  onChanged: _onLoaiMonAnChanged,
                  items: _orderController.dsloaiMonAn.map((loaiMonAn) {
                    return DropdownMenuItem<String>(
                      value: loaiMonAn,
                      child: Text(loaiMonAn),
                    );
                  }).toList(),
                ),
              ],
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                ElevatedButton(
                  onPressed: _toggleSelectedItems,
                  style: ElevatedButton.styleFrom(
                    backgroundColor:
                        showSelectedItems ? Colors.blue : Colors.grey,
                  ),
                  child: const Text('Món Ăn Đã Chọn'),
                ),
                ElevatedButton(
                  onPressed: _toggleAllItems,
                  style: ElevatedButton.styleFrom(
                    backgroundColor: showAllItems ? Colors.blue : Colors.grey,
                  ),
                  child: const Text('Danh Sách Món Ăn'),
                ),
              ],
            ),
          ),
          Expanded(
            child: showAllItems
                ? GridView.builder(
                    gridDelegate:
                        const SliverGridDelegateWithFixedCrossAxisCount(
                      crossAxisCount: 2,
                      childAspectRatio: 0.7,
                      crossAxisSpacing: 10,
                      mainAxisSpacing: 10,
                    ),
                    itemCount: filteredMonAnList.length,
                    itemBuilder: (context, index) {
                      final monAn = filteredMonAnList[index];
                      final quantity = _orderController.quantities[index];

                      return CardProduct(
                        monAn: monAn,
                        quantity: quantity,
                        onIncrement: () {
                          setState(() {
                            if (_orderController.quantities[index] < 10) {
                              _orderController.quantities[index]++;
                            }
                          });
                        },
                        onDecrement: () {
                          setState(() {
                            if (_orderController.quantities[index] > 0) {
                              _orderController.quantities[index]--;
                            }
                          });
                        },
                      );
                    },
                  )
                : FutureBuilder<List<ChiTietDatBan>>(
                    future: selectedItems,
                    builder: (context, snapshot) {
                      final selectedItems = snapshot.data ?? [];
                      return ListView.builder(
                        itemCount: selectedItems.length,
                        itemBuilder: (context, index) {
                          final chiTietDatBan = selectedItems[index];
                          return CardSelectedItem(
                            chiTietDatBan: chiTietDatBan,
                            onDelete: () {
                              xoa1monChiTietDatBan(chiTietDatBan.id);
                            },
                            onSusscec: () {
                              xacnhanramon(chiTietDatBan.id, );
                            },
                          );
                        },
                      );
                    },
                  ),
          ),
          // Chỉ hiển thị OrderFooter khi không tìm kiếm
            OrderFooter(
              chotMonAn: () async {
                // Gọi hàm chốt món ăn
                _orderController.chotMonAn(widget.ban, context);
                setState(() {
                  selectedItems = chitietdatbanController
                      .getChiTietDatBan(widget.ban.banId);
                });
              },
              ban: widget.ban,
              selectedItems: selectedItems,
            ),
        ],
      ),
    );
  }
}
