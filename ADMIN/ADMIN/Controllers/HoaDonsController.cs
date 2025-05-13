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
    public class HoaDonsController : Controller
    {
        private readonly MenuDbContext _context;

        public HoaDonsController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: HoaDons
        public async Task<IActionResult> Index(string searchString, string datePicker, int pageNumber = 1, int pageSize = 10)
        {
            var hoaDons = _context.HoaDons
                                  .Include(h => h.Ban)
                                  .Include(h => h.KhachHang)
                                  .AsQueryable();

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrEmpty(searchString))
            {
                hoaDons = hoaDons.Where(h => h.KhachHang.TenKhachHang.Contains(searchString) || h.HoaDonId.ToString().Contains(searchString));
            }

            // Lọc theo ngày
            if (!string.IsNullOrEmpty(datePicker))
            {
                DateTime selectedDate;
                if (DateTime.TryParse(datePicker, out selectedDate))
                {
                    hoaDons = hoaDons.Where(h => h.NgayTao.Date == selectedDate.Date);
                }
            }

            // Tổng số hóa đơn sau khi lọc
            var totalHoaDons = await hoaDons.CountAsync();

            // Phân trang
            var hoaDonList = await hoaDons
                .OrderByDescending(h => h.NgayTao)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalHoaDons / (double)pageSize);

            // Gửi thông tin về View
            ViewBag.CurrentFilter = searchString;
            ViewBag.DateFilter = datePicker; // Lưu lại ngày đã chọn
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;

            return View(hoaDonList);
        }




        // GET: HoaDons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HoaDons == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.Ban)
                .Include(h => h.KhachHang)
                .FirstOrDefaultAsync(m => m.HoaDonId == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // GET: HoaDons/Create
        public IActionResult Create()
        {
            ViewData["BanId"] = new SelectList(_context.Bans, "BanId", "TenBan");
            ViewData["KhachHangId"] = new SelectList(_context.KhachHangs, "KhachHangId", "TenKhachHang");
            return View();
        }

        // POST: HoaDons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Gửi lại dữ liệu dropdown khi có lỗi
            ViewData["BanId"] = new SelectList(_context.Bans, "BanId", "TenBan", hoaDon.BanId);
            ViewData["KhachHangId"] = new SelectList(_context.KhachHangs, "KhachHangId", "TenKhachHang", hoaDon.KhachHangId);
            return View(hoaDon);
        }

        // GET: HoaDons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HoaDons == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            ViewData["BanId"] = new SelectList(_context.Bans, "BanId", "TenBan", hoaDon.BanId);
            ViewData["KhachHangId"] = new SelectList(_context.KhachHangs, "KhachHangId", "TenKhachHang", hoaDon.KhachHangId);
            return View(hoaDon);
        }

        // POST: HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HoaDonId,KhachHangId,BanId,NgayTao,TongTien")] HoaDon hoaDon)
        {
            if (id != hoaDon.HoaDonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonExists(hoaDon.HoaDonId))
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
            ViewData["BanId"] = new SelectList(_context.Bans, "BanId", "BanId", hoaDon.BanId);
            ViewData["KhachHangId"] = new SelectList(_context.KhachHangs, "KhachHangId", "KhachHangId", hoaDon.KhachHangId);
            return View(hoaDon);
        }

        // GET: HoaDons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HoaDons == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.Ban)
                .Include(h => h.KhachHang)
                .FirstOrDefaultAsync(m => m.HoaDonId == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // POST: HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HoaDons == null)
            {
                return Problem("Entity set 'MenuDbContext.HoaDons'  is null.");
            }
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
          return (_context.HoaDons?.Any(e => e.HoaDonId == id)).GetValueOrDefault();
        }
    }
}
