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
    public class ChietKhausController : Controller
    {
        private readonly MenuDbContext _context;

        public ChietKhausController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: ChietKhaus
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            if (_context.ChietKhau == null)
            {
                return Problem("Entity set 'MenuDbContext.ChietKhau' is null.");
            }

            // Lấy danh sách chiết khấu
            var discounts = _context.ChietKhau.AsQueryable();

            // Áp dụng tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                discounts = discounts.Where(d => d.MaChietKhau.Contains(searchString));
            }

            // Đếm tổng số bản ghi sau khi tìm kiếm
            var totalDiscounts = await discounts.CountAsync();

            // Lấy dữ liệu theo trang
            var discountList = await discounts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Tính tổng số trang
            var totalPages = (int)Math.Ceiling(totalDiscounts / (double)pageSize);

            // Truyền dữ liệu cho View
            ViewBag.CurrentFilter = searchString;  // Từ khóa tìm kiếm hiện tại
            ViewBag.CurrentPage = pageNumber;     // Trang hiện tại
            ViewBag.TotalPages = totalPages;      // Tổng số trang

            return View(discountList);
        }

        // GET: ChietKhaus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ChietKhau == null)
            {
                return NotFound();
            }

            var chietKhau = await _context.ChietKhau
                .FirstOrDefaultAsync(m => m.ChietKhauId == id);
            if (chietKhau == null)
            {
                return NotFound();
            }

            return View(chietKhau);
        }

        // GET: ChietKhaus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChietKhaus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChietKhauId,MaChietKhau,GiaTriChietKhau,NgayBatDau,NgayKetThuc,Soluong")] ChietKhau chietKhau)
        {
            // Kiểm tra ngày bắt đầu và ngày kết thúc
            if (chietKhau.NgayBatDau >= chietKhau.NgayKetThuc)
            {
                ViewBag.ErrorMessage = "Ngày kết thúc không hợp lệ.";
                return View(chietKhau);
            }

            // Kiểm tra mã chiết khấu duy nhất
            bool isMaChietKhauExists = await _context.ChietKhau
                .AnyAsync(c => c.MaChietKhau == chietKhau.MaChietKhau);
            if (isMaChietKhauExists)
            {
                ViewBag.ErrorMessage = "Mã chiết khấu đã tồn tại. Vui lòng nhập mã khác.";
                return View(chietKhau);
            }

            if (ModelState.IsValid)
            {
                _context.Add(chietKhau);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(chietKhau);
        }


        // GET: ChietKhaus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChietKhau == null)
            {
                return NotFound();
            }

            var chietKhau = await _context.ChietKhau.FindAsync(id);
            if (chietKhau == null)
            {
                return NotFound();
            }
            return View(chietKhau);
        }

        // POST: ChietKhaus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChietKhauId,MaChietKhau,GiaTriChietKhau,NgayBatDau,NgayKetThuc")] ChietKhau chietKhau)
        {
            if (id != chietKhau.ChietKhauId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chietKhau);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChietKhauExists(chietKhau.ChietKhauId))
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
            return View(chietKhau);
        }

        // GET: ChietKhaus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChietKhau == null)
            {
                return NotFound();
            }

            var chietKhau = await _context.ChietKhau
                .FirstOrDefaultAsync(m => m.ChietKhauId == id);
            if (chietKhau == null)
            {
                return NotFound();
            }

            return View(chietKhau);
        }

        // POST: ChietKhaus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ChietKhau == null)
            {
                return Problem("Entity set 'MenuDbContext.ChietKhau'  is null.");
            }
            var chietKhau = await _context.ChietKhau.FindAsync(id);
            if (chietKhau != null)
            {
                _context.ChietKhau.Remove(chietKhau);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChietKhauExists(int id)
        {
          return (_context.ChietKhau?.Any(e => e.ChietKhauId == id)).GetValueOrDefault();
        }
    }
}
