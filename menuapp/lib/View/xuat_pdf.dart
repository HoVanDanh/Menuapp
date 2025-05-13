import 'package:flutter/material.dart';
import 'package:menuapp/Model/getHoadon.dart';
import 'package:flutter/services.dart';
import 'package:menuapp/View/homepage/homePage.dart';
import 'package:menuapp/controller/payBill_controller.dart';
import 'package:menuapp/provider/accountProvider.dart';
import 'package:pdf/widgets.dart' as pw;
import 'package:pdf/pdf.dart';
import 'package:printing/printing.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:menuapp/Controller/hoadon_controller.dart';

class HoaDonScreen extends StatelessWidget {
  final int hoaDonId;
  final String chieckhau;

  const HoaDonScreen(
      {Key? key, required this.hoaDonId, required this.chieckhau})
      : super(key: key);
  Future<HoaDon?> _layHoaDon() async {
    try {
      return await hoadonController.layhoadon(hoaDonId);
    } catch (e) {
      print('Lỗi lấy hóa đơn: $e');
      return null;
    }
  }

  @override
  Widget build(BuildContext context) {
    final acc = Provider.of<accountnprovider>(context).user;
    return Scaffold(
      body: FutureBuilder<HoaDon?>(
        // Gọi hàm lấy hóa đơn
        future: _layHoaDon(),
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          } else if (snapshot.hasError) {
            return Center(child: Text('Lỗi: ${snapshot.error}'));
          } else if (!snapshot.hasData) {
            return const Center(child: Text('Không có dữ liệu hóa đơn'));
          }

          final hoaDon = snapshot.data!;
          return FutureBuilder<Uint8List>(
            future: _generatePdf(hoaDon, acc),
            builder: (context, pdfSnapshot) {
              if (pdfSnapshot.connectionState == ConnectionState.waiting) {
                return const Center(child: CircularProgressIndicator());
              } else if (pdfSnapshot.hasError) {
                return Center(
                    child: Text('Lỗi xuất PDF: ${pdfSnapshot.error}'));
              } else if (!pdfSnapshot.hasData) {
                return const Center(child: Text('Không thể tạo PDF'));
              }

              final pdfData = pdfSnapshot.data!;
              return Column(
                children: [
                  Expanded(
                    child: PdfPreview(
                      build: (format) => pdfData,
                      canChangePageFormat: false,
                      canDebug: false,
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.all(8.0),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                      children: [
                        ElevatedButton.icon(
                          onPressed: () async {
                            Navigator.pushReplacementNamed(
                                context, '/HomePage');
                          },
                          label: const Text('Xác nhận'),
                        ),
                        ElevatedButton.icon(
                          onPressed: () {
                            Printing.sharePdf(
                              bytes: pdfData,
                              filename: 'HoaDon_${hoaDon.hoaDonId}.pdf',
                            );
                          },
                          label: const Text('In'),
                        ),
                      ],
                    ),
                  ),
                ],
              );
            },
          );
        },
      ),
    );
  }

  // xuat thong tin hoa don
  Future<Uint8List> _generatePdf(HoaDon hoaDon, acc) async {
    final fontData = await rootBundle.load('assets/fonts/Roboto-Regular.ttf');
    final ttf = pw.Font.ttf(fontData);
    final fontDataBold = await rootBundle.load('assets/fonts/Roboto-Bold.ttf');
    final ttfBold = pw.Font.ttf(fontDataBold);

    final pdf = pw.Document();

    pdf.addPage(
      pw.MultiPage(
        pageFormat: PdfPageFormat.a4,
        build: (context) => [
          pw.Center(
            child: pw.Column(
              children: [
                pw.Text(
                  'thông tin hóa đơn',
                  style: pw.TextStyle(font: ttfBold, fontSize: 24),
                ),
                pw.SizedBox(height: 20),
                pw.Text("nhan vien lap hd: ${acc.nhanVien.tenNhanVien}"),
                pw.Text(
                  'ID HD: ${hoaDon.hoaDonId}',
                ),
                pw.Text(
                  'Khách Hàng: ${hoaDon.tenKhachHanh}',
                  style: pw.TextStyle(font: ttf),
                ),
                pw.Text(
                  'Ngày Tạo: ${DateFormat('dd/MM/yyyy HH:mm').format(hoaDon.ngayTao)}',
                  style: pw.TextStyle(font: ttf),
                ),
                pw.SizedBox(height: 20),
                pw.Text(
                  'Chi Tiết Hóa Đơn:',
                  style: pw.TextStyle(font: ttfBold, fontSize: 18),
                ),
                pw.SizedBox(height: 10),
                pw.Table.fromTextArray(
                  headers: ['Tên Món', 'Số Lượng', 'Giá', 'Tổng'],
                  data: hoaDon.chiTietHoaDons
                      .map((chiTiet) => [
                            chiTiet.tenMon,
                            chiTiet.soLuong.toString(),
                            NumberFormat.currency(locale: 'vi_VN', symbol: '₫')
                                .format(chiTiet.gia),
                            NumberFormat.currency(locale: 'vi_VN', symbol: '₫')
                                .format(chiTiet.soLuong * chiTiet.gia),
                          ])
                      .toList(),
                  cellStyle: pw.TextStyle(font: ttf),
                  headerStyle: pw.TextStyle(font: ttfBold),
                  border: pw.TableBorder.all(),
                  headerDecoration: pw.BoxDecoration(color: PdfColors.grey300),
                  cellPadding: const pw.EdgeInsets.all(8),
                ),
                pw.SizedBox(height: 10),
                pw.Align(
                  alignment: pw.Alignment.centerRight,
                  child: pw.Text(
                    'Chiết Khấu: $chieckhau  ₫',
                    style: pw.TextStyle(font: ttfBold, fontSize: 18),
                  ),
                ),
                pw.SizedBox(height: 20),
                pw.Align(
                  alignment: pw.Alignment.centerRight,
                  child: pw.Text(
                    'Tổng Tiền: ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(hoaDon.tongTien)}',
                    style: pw.TextStyle(font: ttfBold, fontSize: 18),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
    return pdf.save();
  }
}
