using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADMIN.Repository;
using ADMIN.Repository.Models;

namespace ADMIN.Controllers
{
    public class KhachHangsController : Controller
    {
        private readonly MenuDbContext _context;

        public KhachHangsController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: KhachHangs
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            // Lấy danh sách khách hàng từ cơ sở dữ liệu
            var customers = _context.KhachHangs.AsQueryable(); // Lọc khách hàng có trạng thái hoạt động

            // Nếu có từ khóa tìm kiếm, áp dụng bộ lọc
            if (!string.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(kh => kh.TenKhachHang.Contains(searchString) || kh.SoDienThoai.Contains(searchString));
            }

            // Đếm tổng số khách hàng sau khi áp dụng bộ lọc
            var totalCustomers = await customers.CountAsync();

            // Áp dụng phân trang
            var customerList = await customers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Tính tổng số trang
            var totalPages = (int)Math.Ceiling(totalCustomers / (double)pageSize);

            // Truyền dữ liệu vào View thông qua ViewBag
            ViewBag.CurrentFilter = searchString; // Lưu giá trị tìm kiếm hiện tại
            ViewBag.CurrentPage = pageNumber;    // Trang hiện tại
            ViewBag.TotalPages = totalPages;     // Tổng số trang

            // Trả về View với danh sách khách hàng đã được phân trang
            return View(customerList);
        }


        // GET: KhachHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.KhachHangId == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: KhachHangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KhachHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KhachHangId,TenKhachHang,SoDienThoai,DiaChi")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        // POST: KhachHangs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KhachHangId,TenKhachHang,SoDienThoai,DiaChi")] KhachHang khachHang)
        {
            if (id != khachHang.KhachHangId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khachHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhachHangExists(khachHang.KhachHangId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.KhachHangId == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KhachHangs == null)
            {
                return Problem("Entity set 'MenuDbContext.KhachHangs'  is null.");
            }
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHangs.Remove(khachHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(int id)
        {
          return (_context.KhachHangs?.Any(e => e.KhachHangId == id)).GetValueOrDefault();
        }
    }
}
