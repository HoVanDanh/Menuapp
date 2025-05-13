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
    public class AccountsController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public AccountsController(MenuDbContext context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public async Task<IActionResult> login (loginDTO login)
        {
            if(login.TenDangNhap ==null && login.MatKhau ==null)
            {
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không chính xác");
            }
            // truy vaans thong tin danng nhap bang ten thong tin ten dang nhap va mat khau
            var account = await _context.Accounts
                   .Include(a => a.NhanVien) // Bao gồm thông tin nhân viên
                   .FirstOrDefaultAsync(a => a.TenDangNhap == login.TenDangNhap && a.MatKhau == login.MatKhau);
            if (account == null)
            {
                return Unauthorized("Tên đăng nhập hoặc mật khẩu không chính xác");

            }
            var accountDTO = new AccountDTO
            {
                AccountId = account.AccountId,
                TenDangNhap = account.TenDangNhap,
                MatKhau= account.MatKhau,
                PhanQuyen = account.PhanQuyen,
                NhanVien = new NhanVienDTO // Tạo đối tượng NhanVienDTO
                {
                    NhanVienId = account.NhanVien.NhanVienId,
                    TenNhanVien = account.NhanVien.TenNhanVien,
                    SoDienThoai = account.NhanVien.SoDienThoai,
                    DiaChi = account.NhanVien.DiaChi,
                    LoaiNhanVienId = account.NhanVien.LoaiNhanVienId
                }
            };
            return Ok(accountDTO);
        }
       
        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
