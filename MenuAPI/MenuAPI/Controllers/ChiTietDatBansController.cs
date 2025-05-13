using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPI.Models;
using MenuAPI.DTO;
using MenuAPI.Constants;

namespace MenuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietDatBansController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public ChiTietDatBansController(MenuDbContext context)
        {
            _context = context;
        }
        // lay cac mon an tu chi tiet dat ba cua ban 

        [HttpGet("laychitietmonan/{banId}")]
        public async Task<ActionResult<List<ChitiecdatbanDTO>>> GetChiTietMonAnCuaBan(int banId)
        {
            if (_context.ChiTietDatBans == null)
            {
                return NotFound();
            }

            // Truy vấn để lấy tất cả chi tiết món ăn của bàn cụ thể
            var chiTietDatBans = await _context.ChiTietDatBans
                .Where(ctd => ctd.BanId == banId) // Lọc theo banId
                .ToListAsync(); // Lấy tất cả các kết quả

            if (chiTietDatBans == null || !chiTietDatBans.Any())
            {
                return NotFound("Không tìm thấy món ăn cho bàn này.");
            }

            // Tạo danh sách DTO để trả về dữ liệu
            var result = new List<ChitiecdatbanDTO>();
           
            foreach (var ctd in chiTietDatBans)
            {
                // Lấy tên món ăn từ bảng MonAn
                var monAn = await _context.MonAns.FindAsync(ctd.MonAnId);

                result.Add(new ChitiecdatbanDTO
                {
                    Id = ctd.Id,
                    BanId = ctd.BanId,
                    MonAnId = ctd.MonAnId,
                    TenMonAn = monAn?.TenMon, // Lấy tên món ăn
                    SoLuong = ctd.SoLuong,
                    Gia = ctd.Gia,
                    TrangThai= ctd.TrangThai,
                    // thời gian còn lạ để ra món 
                    TimeConLai = thoigianconlai(ctd.ThoiGianTao, monAn.LoaiMonAnId)
                    
                });
            }

            return Ok(result);
        }
        // hàm tính thời gian ra món 
        private int thoigianconlai(DateTime thoigiantao, int loaimon)
        {
            int thoigianchuanbi = loaimon switch
            {
                1 => Constants.Constants.TimeMonChinh,

                2 => Constants.Constants.timeMonKhaivi,

                4 => Constants.Constants.timeThucUong
            };
            var thoigianHienTai = DateTime.Now;
            var thoigiandatroi = (int)(thoigianHienTai - thoigiantao).TotalMinutes;
            var timeconlai = Math.Max(thoigianchuanbi - thoigiandatroi, 0);
            return timeconlai;
        }


        // tao cac mon an cho chi tiet dat ban 
        [HttpPost ("taochitiecdatban")]
        public async Task<ActionResult<ChiTietDatBan>> PostChiTietDatBan(ChiTietDatBan chiTietDatBan)
        {
          if (_context.ChiTietDatBans == null)
          {
              return Problem("Entity set 'MenuDbContext.ChiTietDatBans'  is null.");
          }
            chiTietDatBan.ThoiGianTao = DateTime.Now;
            _context.ChiTietDatBans.Add(chiTietDatBan);
            await _context.SaveChangesAsync();

            return Ok(chiTietDatBan);
        }
        // xoa cac mon an khi ban dc thanh toan 
        [HttpDelete("xoamonancuaban/{banId}")]
        public async Task<IActionResult> DeleteChiTietDatBanTheoBanId(int banId)
        {
            if (_context.ChiTietDatBans == null)
            {
                return NotFound();
            }
            // lay cac cac chi tiet dat ban theo ban id 
            var chiTietDatBanList = _context.ChiTietDatBans
                                             .Where(c => c.BanId == banId)
                                             .ToList();

            if (chiTietDatBanList == null || !chiTietDatBanList.Any())
            {
                return NotFound();
            }
           // await GiamNguyenLieu(chiTietDatBanList);
            _context.ChiTietDatBans.RemoveRange(chiTietDatBanList);
            await _context.SaveChangesAsync();

            return NoContent();
        }
      
        // xóa món ăn cho của bàn theo ban id và món ăn 
        [HttpDelete("xoa1monancuaban/{id}")]
        public async Task<IActionResult> DeleteChiTietDatBanTheoBanIdVaMonAnId(int id)
        {
            if (_context.ChiTietDatBans == null)
            {
                return NotFound();
            }

            // Tìm chi tiết đặt bàn theo banId và monAnId
            var chiTietDatBan = _context.ChiTietDatBans
                                         .FirstOrDefault(c => c.Id == id);

            if (chiTietDatBan == null)
            {
                return NotFound();
            }

            // Xóa chi tiết đặt bàn đã tìm thấy
            _context.ChiTietDatBans.Remove(chiTietDatBan);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPost("xacnhanmonan/{id}")]
        public async Task<IActionResult> XacNhanMonAn(int id)
        {
            if (_context.ChiTietDatBans == null)
            {
                return NotFound();
            }

            // Tìm chi tiết đặt bàn theo ID
            var chiTietDatBan = await _context.ChiTietDatBans.FindAsync(id);

            if (chiTietDatBan == null)
            {
                return NotFound();
            }

            // Cập nhật trạng thái
            chiTietDatBan.TrangThai = true;
            await GiamNguyenLieu(chiTietDatBan.MonAnId, chiTietDatBan.SoLuong);
            // Lưu thay đổi
            await _context.SaveChangesAsync();

            return Ok();
        }

        // ham giam nguyen lieu mon an xuong db 
        //private async Task GiamNguyenLieu(List<ChiTietDatBan> chiTietDatBans)
        //{
        //    foreach (var chiTiet in chiTietDatBans)
        //    {

        //        var monAn = await _context.MonAns
        //            .Include(m => m.NguyenLieuMonAns) 
        //            .FirstOrDefaultAsync(m => m.MonAnId == chiTiet.MonAnId);

        //        if (monAn != null)
        //        {
        //            foreach (var nguyenLieuMonAn in monAn.NguyenLieuMonAns)
        //            {
        //                var nguyenLieu = await _context.NguyenLieus
        //                    .FirstOrDefaultAsync(n => n.NguyenLieuId == nguyenLieuMonAn.NguyenLieuId);

        //                if (nguyenLieu != null)
        //                {
        //                    nguyenLieu.SoLuong -= nguyenLieuMonAn.SoLuong * chiTiet.SoLuong;

        //                    if (nguyenLieu.SoLuong < 0)
        //                    {
        //                        nguyenLieu.SoLuong = 0;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //}
        private async Task GiamNguyenLieu(int monAnId, int soLuongMonAn)
        {
            var monAn = await _context.MonAns
                .Include(m => m.NguyenLieuMonAns) // Bao gồm danh sách nguyên liệu món ăn
                .FirstOrDefaultAsync(m => m.MonAnId == monAnId);

            if (monAn != null)
            {
                // Duyệt qua từng nguyên liệu món ăn liên quan đến món ăn này
                foreach (var nguyenLieuMonAn in monAn.NguyenLieuMonAns)
                {
                    var nguyenLieu = await _context.NguyenLieus
                        .FirstOrDefaultAsync(n => n.NguyenLieuId == nguyenLieuMonAn.NguyenLieuId);

                    if (nguyenLieu != null)
                    {
                        // Giảm số lượng nguyên liệu theo công thức
                        nguyenLieu.SoLuong -= nguyenLieuMonAn.SoLuong * soLuongMonAn;

                        // Đảm bảo số lượng nguyên liệu không bị âm
                        if (nguyenLieu.SoLuong < 0)
                        {
                            nguyenLieu.SoLuong = 0;
                        }
                    }
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Món ăn không tồn tại hoặc không có nguyên liệu liên quan.");
            }
        }
    }
}
