// import 'package:flutter/material.dart';

// class ProfileEditScreen extends StatefulWidget {
//   const ProfileEditScreen({super.key});

//   @override
//   _ProfileEditScreenState createState() => _ProfileEditScreenState();
// }

// class _ProfileEditScreenState extends State<ProfileEditScreen> {
//   @override
//   Widget build(BuildContext context) {
//     return Scaffold(
//       appBar: AppBar(
//         title: const Text('Edit Profile'),
//         centerTitle: true,
//       ),
//       body: SingleChildScrollView(
//         padding: const EdgeInsets.all(16.0),
//         child: Column(
//           children: [
//             // Profile Picture
//             const Stack(
//               alignment: Alignment.center,
//               children: [
//                 CircleAvatar(
//                   radius: 50,
//                   backgroundImage: AssetImage('assets/profile_placeholder.jpg'),
//                 ),
//                 Positioned(
//                   bottom: 0,
//                   right: 0,
//                   child: Icon(Icons.edit, size: 24),
//                 ),
//               ],
//             ),
//             const SizedBox(height: 24),

//             // Display Name Field
//             const TextField(
//               decoration: InputDecoration(
//                 labelText: 'Display Name',
//                 border: OutlineInputBorder(),
//               ),
//             ),
//             const SizedBox(height: 16),

//             // Username Field
//             const TextField(
//               decoration: InputDecoration(
//                 labelText: 'Username',
//                 border: OutlineInputBorder(),
//               ),
//             ),
//             const SizedBox(height: 16),

//             // Email Field
//             const TextField(
//               decoration: InputDecoration(
//                 labelText: 'Email',
//                 border: OutlineInputBorder(),
//               ),
//             ),
//             const SizedBox(height: 16),

//             // Phone Number Field
//             const TextField(
//               decoration: InputDecoration(
//                 labelText: 'Phone Number',
//                 border: OutlineInputBorder(),
//               ),
//             ),
//             const SizedBox(height: 16),

//             // Gender Field
//             const TextField(
//               decoration: InputDecoration(
//                 labelText: 'Gender',
//                 border: OutlineInputBorder(),
//               ),
//             ),
//             const SizedBox(height: 24),

//             // Update Button
//             ElevatedButton(
//               onPressed: () {
//                 // Handle update action
//               },
//               style: ElevatedButton.styleFrom(
//                 minimumSize: const Size(double.infinity, 48), // Full-width button
//               ),
//               child: const Text('Update'),
//             ),
//           ],
//         ),
//       ),
//     );
//   }
// }
