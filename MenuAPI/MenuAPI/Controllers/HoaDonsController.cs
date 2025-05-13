using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPI.Models;
using MenuAPI.DTO;

namespace MenuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonsController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public HoaDonsController(MenuDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<HoaDonDTO>> GetHoaDonById(int id)
        {
            if (_context.HoaDons == null)
            {
                return Problem("Entity set 'MenuDbContext.HoaDons' is null.");
            }

            // Tìm hóa đơn và bao gồm chi tiết hóa đơn và thông tin món ăn
            var hoaDon = await _context.HoaDons
                .Include(hd => hd.ChiTietHoaDons)
                .ThenInclude(ct => ct.MonAn) // Bao gồm thông tin món ăn
                .FirstOrDefaultAsync(hd => hd.HoaDonId == id);

            if (hoaDon == null)
            {
                return NotFound(); 
            }
            var khachHang = await _context.KhachHangs.FindAsync(hoaDon.KhachHangId);
            // Chuyển đổi hóa đơn thành DTO
            var hoaDonDto = new HoaDonDTO
            {
                HoaDonId = hoaDon.HoaDonId,
                KhachHangId = hoaDon.KhachHangId,
                TenKhachHanh = khachHang?.TenKhachHang,
                BanId = hoaDon.BanId,
                NgayTao = hoaDon.NgayTao,
                TongTien = hoaDon.TongTien,
                ChiTietHoaDons = hoaDon.ChiTietHoaDons.Select(ct => new ChiTietHoaDonDTO
                {
                    ChiTietHoaDonId = ct.ChiTietHoaDonId,
                    HoaDonId=hoaDon.HoaDonId,
                    MonAnId = ct.MonAnId,
                    TenMon = ct.MonAn?.TenMon,
                    SoLuong = ct.SoLuong,
                    Gia = ct.Gia,
                }).ToList()
            };

            return Ok(hoaDonDto); // Trả về thông tin hóa đơn và chi tiết hóa đơn
        }

        [HttpPost("TaoHoaDon")]
        public async Task<ActionResult<HoaDonDTO>> TaoHoaDon([FromBody] TaoHoaDon taoHoaDon)
        {
            if (_context.HoaDons == null)
            {
                return Problem("Entity set 'MenuDbContext.HoaDons' is null.");
            }

            var khachHang = taoHoaDon.KhachHang;
            int khachHangId = 1; // khowi taoj khach hang mac dinh cho hoa don 

            if (khachHang != null && !string.IsNullOrEmpty(khachHang.SoDienThoai))
            {
                var existingKhachHang = await _context.KhachHangs
                    .FirstOrDefaultAsync(k => k.SoDienThoai == khachHang.SoDienThoai);

                if (existingKhachHang != null)
                {
                    khachHangId = existingKhachHang.KhachHangId;
                }
                else
                {
                    var newKhachHang = new KhachHang
                    {
                        TenKhachHang = khachHang.TenKhachHang,
                        SoDienThoai = khachHang.SoDienThoai,
                        DiaChi = khachHang.DiaChi
                    };
                    _context.KhachHangs.Add(newKhachHang);
                    await _context.SaveChangesAsync();
                    khachHangId = newKhachHang.KhachHangId;
                }
            }

            var hoaDon = new HoaDon
            {
                KhachHangId = khachHangId,
                BanId = taoHoaDon.HoaDon.BanId,
                NgayTao = DateTime.Now,
                TongTien = taoHoaDon.HoaDon.TongTien,
                TrangThai = true
                
            };

            hoaDon.ChiTietHoaDons = taoHoaDon.HoaDon.ChiTietHoaDons
                .Select(chiTietDto => new ChiTietHoaDon
                {
                    MonAnId = chiTietDto.MonAnId,
                    SoLuong = chiTietDto.SoLuong,
                    Gia = chiTietDto.Gia
                }).ToList();

            _context.HoaDons.Add(hoaDon);
            await _context.SaveChangesAsync();

            var hoaDonDto = new HoaDonDTO
            {
                HoaDonId = hoaDon.HoaDonId,
                KhachHangId = hoaDon.KhachHangId,
                BanId = hoaDon.BanId,
                NgayTao = hoaDon.NgayTao,
                TongTien = hoaDon.TongTien,
                ChiTietHoaDons = taoHoaDon.HoaDon.ChiTietHoaDons
            };

            return CreatedAtAction(nameof(GetHoaDonById), new { id = hoaDon.HoaDonId }, hoaDonDto);
        }

        [HttpDelete("HuyHoaDon/{hoaDonId}")]
        public async Task<IActionResult> HuyHoaDon(int hoaDonId)
        {
            if (_context.HoaDons == null)
            {
                return Problem("Entity set 'MenuDbContext.HoaDons' is null.");
            }

            // Tìm hóa đơn theo ID
            var hoaDon = await _context.HoaDons
                .Include(hd => hd.ChiTietHoaDons)
                .FirstOrDefaultAsync(hd => hd.HoaDonId == hoaDonId);

            if (hoaDon == null)
            {
                return NotFound(new { Message = "Hóa đơn không tồn tại." });
            }

            // Xóa chi tiết hóa đơn
            _context.ChiTietHoaDons.RemoveRange(hoaDon.ChiTietHoaDons);

            // Xóa hóa đơn
            _context.HoaDons.Remove(hoaDon);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Hóa đơn đã được hủy thành công." });
        }
        // xac nhaa thanh toan 
        [HttpPut("XacNhanThanhToan/{hoaDonId}")]
        public async Task<IActionResult> XacNhanThanhToan(int hoaDonId)
        {
            if (_context.HoaDons == null)
            {
                return Problem("Entity set 'MenuDbContext.HoaDons' is null.");
            }

            // Tìm hóa đơn theo ID
            var hoaDon = await _context.HoaDons.FindAsync(hoaDonId);

            if (hoaDon == null)
            {
                return NotFound(new { Message = "Hóa đơn không tồn tại." });
            }

            // Cập nhật trạng thái thành true (đã thanh toán)
            hoaDon.TrangThai = true;
            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Entry(hoaDon).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Xác nhận thanh toán thành công.", HoaDonId = hoaDonId });
        }
        private bool HoaDonExists(int id)
        {
            return (_context.HoaDons?.Any(e => e.HoaDonId == id)).GetValueOrDefault();
        }
         
    }
}
