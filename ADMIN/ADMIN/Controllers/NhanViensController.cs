using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADMIN.Repository;
using ADMIN.Repository.Models;
using System.Text;

namespace ADMIN.Controllers
{
    public class NhanViensController : Controller
    {
        private readonly MenuDbContext _context;

        public NhanViensController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: NhanViens
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var employees = _context.NhanViens
            .Include(nv => nv.LoaiNhanVien)
            .Where(nv => nv.TrangThai != false)
            .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.TenNhanVien.Contains(searchString));
            }

            var totalEmployees = await employees.CountAsync();

            var employeeList = await employees
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalEmployees / (double)pageSize);

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewData["LoaiNhanVienId"] = new SelectList(_context.LoaiNhanViens, "LoaiNhanVienId", "TenLoai");
            return View(employeeList);

        }

        // GET: NhanViens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .Include(n => n.LoaiNhanVien)
                .FirstOrDefaultAsync(m => m.NhanVienId == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // GET: NhanViens/Create
        public IActionResult Create()
        {
            ViewData["LoaiNhanVienId"] = new SelectList(_context.LoaiNhanViens, "LoaiNhanVienId", "TenLoai");
            return View();
        }

        // POST: NhanViens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVien nhanVien, string TenDangNhap, string MatKhau)
        {
            if (string.IsNullOrWhiteSpace(TenDangNhap))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(MatKhau))
            {
                ModelState.AddModelError("MatKhau", "Mật khẩu không được để trống.");
            }

            // Kiểm tra trùng lặp tên đăng nhập
            if (await _context.Accounts.AnyAsync(a => a.TenDangNhap == TenDangNhap))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                ViewData["LoaiNhanVienId"] = new SelectList(_context.LoaiNhanViens, "LoaiNhanVienId", "TenLoai", nhanVien.LoaiNhanVienId);
                return View(nhanVien);
            }

            _context.Add(nhanVien);
            await _context.SaveChangesAsync();


            // Tạo tài khoản
            var account = new Account
            {
                TenDangNhap = TenDangNhap,
                MatKhau = MatKhau,
                NhanVienId = nhanVien.NhanVienId,
                PhanQuyen = nhanVien.LoaiNhanVienId
            };

            _context.Add(account);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Tạo nhân viên và tài khoản thành công!";
            return RedirectToAction(nameof(Index));

            //@*ViewData["LoaiNhanVienId"] = new SelectList(_context.LoaiNhanViens, "LoaiNhanVienId", "TenLoai", nhanVien.LoaiNhanVienId);
            //return View(nhanVien);*@
        }



        // GET: NhanViens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            // Lấy thông tin tài khoản liên quan đến NhanVien
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.NhanVienId == nhanVien.NhanVienId);
            if (account != null)
            {
                ViewBag.TenDangNhap = account.TenDangNhap;
                ViewBag.MatKhau = account.MatKhau;
            }
            else
            {
                ViewBag.TenDangNhap = "";
                ViewBag.MatKhau = string.Empty;
            }

            ViewData["LoaiNhanVienId"] = new SelectList(_context.LoaiNhanViens, "LoaiNhanVienId", "TenLoai", nhanVien.LoaiNhanVienId);
            return View(nhanVien);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NhanVienId,TenNhanVien,SoDienThoai,DiaChi,LoaiNhanVienId")] NhanVien nhanVien, string MatKhau)
        {
            if (id != nhanVien.NhanVienId)
            {
                return NotFound();
            }

            try
            {
                // Tìm nhân viên hiện tại
                var existingNhanVien = await _context.NhanViens.FindAsync(id);
                if (existingNhanVien == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin nhân viên
                existingNhanVien.TenNhanVien = nhanVien.TenNhanVien;
                existingNhanVien.SoDienThoai = nhanVien.SoDienThoai;
                existingNhanVien.DiaChi = nhanVien.DiaChi;
                existingNhanVien.LoaiNhanVienId = nhanVien.LoaiNhanVienId;

                // Cập nhật tài khoản
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.NhanVienId == nhanVien.NhanVienId);
                if (account != null)
                {
                    account.MatKhau = string.IsNullOrEmpty(MatKhau) ? account.MatKhau : MatKhau;
                    account.PhanQuyen = nhanVien.LoaiNhanVienId;
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhanVienExists(nhanVien.NhanVienId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        // GET: NhanViens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NhanViens == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .Include(n => n.LoaiNhanVien)
                .FirstOrDefaultAsync(m => m.NhanVienId == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NhanViens == null)
            {
                return Problem("Entity set 'MenuDbContext.NhanViens' is null.");
            }

            // Tìm nhân viên theo id
            var nhanVien = await _context.NhanViens.FindAsync(id);

            if (nhanVien != null)
            {
                // Tìm tài khoản liên kết với nhân viên này
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.NhanVienId == nhanVien.NhanVienId);

                //if (account != null)
                //{
                //    // Xóa tài khoản liên kết nếu cần
                //    _context.Accounts.Remove(account);
                //}

                // Chuyển trạng thái của nhân viên thành false
                nhanVien.TrangThai = false;

                // Lưu thay đổi
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        private bool NhanVienExists(int id)
        {
          return (_context.NhanViens?.Any(e => e.NhanVienId == id)).GetValueOrDefault();
        }
    }
}
