// import 'package:flutter/material.dart';
// import 'package:menuapp/controller/monan_controller.dart';

// FutureBuilder<int> SoLuongPhanConLai(int monAnId) {
//   return FutureBuilder<int>(
//     future: monanController.getSoLuongPhanConLai(monAnId),  
//     builder: (context, snapshot) {
//       if (snapshot.connectionState == ConnectionState.waiting) {
//         return CircularProgressIndicator();
//       } else if (snapshot.hasError) {
//         return Text('Lỗi: ${snapshot.error}');
//       } else if (!snapshot.hasData) {
//         return Text('Không có dữ liệu');
//       } else {
//         final soLuongPhanConLai = snapshot.data!;
//         return 
//             Text(
//               ' còn : $soLuongPhanConLai',
//               style: const TextStyle(
//                 fontSize: 16,
//                 color: Colors.black54,
//               ),
//             );
//       }
//     },
//   );
// }