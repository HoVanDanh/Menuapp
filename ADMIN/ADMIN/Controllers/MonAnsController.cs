using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADMIN.Repository;
using ADMIN.Repository.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ADMIN.Controllers
{
    public class MonAnsController : Controller
    {
        private readonly MenuDbContext _context;

        public MonAnsController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: MonAns
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 5)
        {
            // Lấy dữ liệu từ cơ sở dữ liệu
            var menuDbContext = _context.MonAns.Include(m => m.LoaiMonAn).AsQueryable();

            // Lọc dữ liệu nếu có từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                menuDbContext = menuDbContext.Where(m => m.TenMon.Contains(searchString) ||
                                                         m.LoaiMonAn.TenLoai.Contains(searchString));
            }

            // Tính tổng số món ăn
            var totalItems = await menuDbContext.CountAsync();

            // Lấy dữ liệu trang hiện tại
            var menuItems = await menuDbContext
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Tính số trang
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Truyền dữ liệu vào ViewBag
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewData["CurrentFilter"] = searchString; // Lưu lại từ khóa tìm kiếm để hiển thị lại trong form

            return View(menuItems);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MonAns == null)
            {
                return NotFound();
            }

            var monAn = await _context.MonAns
                .Include(m => m.LoaiMonAn)
                .Include(m => m.NguyenLieuMonAns)
                    .ThenInclude(n => n.NguyenLieu)
                .FirstOrDefaultAsync(m => m.MonAnId == id);

            if (monAn == null)
            {
                return NotFound();
            }
            ViewData["LoaiMonAnId"] = new SelectList(_context.LoaiMonAns, "LoaiMonAnId", "TenLoai");
            return View(monAn);
        }

        // GET: MonAns/Create
        public IActionResult Create()
        {
            var nguyenLieus = _context.NguyenLieus.Select(n => new NguyenLieu
            {
                NguyenLieuId = n.NguyenLieuId, 
                TenNguyenLieu = n.TenNguyenLieu ,
                DonVi = n.DonVi
            }).ToList();

            ViewBag.NguyenLieus = nguyenLieus;
            ViewData["LoaiMonAnId"] = new SelectList(_context.LoaiMonAns, "LoaiMonAnId", "TenLoai");
            return View(new MonAn());
        }
        // thêm món ăn mới 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonAn monAn, NguyenLieuMonAn[] NguyenLieuMonAns, IFormFile imgFile)
        {
            // Xử lý tải lên hình ảnh
            if (imgFile != null && imgFile.Length > 0)
            {// Tạo tên duy nhất kèm tên file gốc
                var guid = Guid.NewGuid().ToString();
                var originalFileName = Path.GetFileNameWithoutExtension(imgFile.FileName);
                var extension = Path.GetExtension(imgFile.FileName);
                var filename = $"{guid}_{originalFileName}{extension}";

                var uploadsFolder = Path.Combine("D:\\DATN\\img");
                var filePath = Path.Combine(uploadsFolder, filename);

                // Tải lên hình ảnh
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imgFile.CopyToAsync(stream);
                }
                //lưu đường dãn vào db
                monAn.Img = Path.Combine("/img/", filename); ; 
            }

            // Lưu món ăn
            _context.MonAns.Add(monAn);
            await _context.SaveChangesAsync();

            // Chuyển hướng về danh sách món ăn
            return RedirectToAction(nameof(Index)); 
        }

        // lấy thông tin món ăn cần chỉ sửa món ăn
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MonAns == null)
            {
                return NotFound();
            }

            var monAn = await _context.MonAns
                .Include(m => m.NguyenLieuMonAns) // Bao gồm nguyên liệu món ăn
                .ThenInclude(nlm => nlm.NguyenLieu) // Bao gồm thông tin nguyên liệu
                .FirstOrDefaultAsync(m => m.MonAnId == id);

            if (monAn == null)
            {
                return NotFound();
            }

            ViewData["LoaiMonAnId"] = new SelectList(_context.LoaiMonAns, "LoaiMonAnId", "TenLoai", monAn.LoaiMonAnId);
            ViewBag.NguyenLieus = _context.NguyenLieus.Select(n => new NguyenLieu
            {
                NguyenLieuId = n.NguyenLieuId,
                TenNguyenLieu = n.TenNguyenLieu,
                DonVi = n.DonVi
            }).ToList();

            return View(monAn);
        }

        // Xử lý chỉnh sửa thông tin món ăn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MonAn monAn, NguyenLieuMonAn[] NguyenLieuMonAns, IFormFile imgFile)
        {
            if (id != monAn.MonAnId)
            {
                return NotFound();
            }
         
            try
            {
                // Lấy thông tin món ăn hiện tại từ cơ sở dữ liệu
                var existingMonAn = await _context.MonAns
                    .Include(m => m.NguyenLieuMonAns) // Bao gồm danh sách nguyên liệu của món ăn
                    .FirstOrDefaultAsync(m => m.MonAnId == id);

                if (existingMonAn == null)
                {
                    return NotFound();
                }

                // Xử lý cập nhật hình ảnh nếu có, nếu không giữ lại ảnh cũ
                if (imgFile != null && imgFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var originalFileName = Path.GetFileNameWithoutExtension(imgFile.FileName);
                    var extension = Path.GetExtension(imgFile.FileName);
                    var filename = $"{guid}_{originalFileName}{extension}";

                    var uploadsFolder = Path.Combine("D:\\DATN\\img");
                    var filePath = Path.Combine(uploadsFolder, filename);

                    // Lưu ảnh vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(stream);
                    }
                    monAn.Img = Path.Combine("/img/", filename);
                }
                else
                {
                    monAn.Img = existingMonAn.Img; // Giữ lại ảnh cũ nếu không có ảnh mới
                }

                // Cập nhật thông tin món ăn (trừ nguyên liệu)
                _context.Entry(existingMonAn).CurrentValues.SetValues(monAn);

                // *** Xóa toàn bộ nguyên liệu của món ăn ***
                var existingNguyenLieus = _context.NguyenLieuMonAns
                    .Where(n => n.MonAnId == monAn.MonAnId)
                    .ToList();
                //_context.NguyenLieuMonAns.RemoveRange(existingNguyenLieus);

                // *** Thêm lại danh sách nguyên liệu mới ***
                foreach (var nguyenLieu in NguyenLieuMonAns)
                {
                    nguyenLieu.MonAnId = monAn.MonAnId;
                    _context.NguyenLieuMonAns.Add(nguyenLieu);
                }

                // Lưu thay đổi
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonAnExists(monAn.MonAnId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        // hủy nguyên liệu món ăn 
        [HttpPost]
        public IActionResult RemoveNguyenLieu(int monAnId, int nguyenLieuId)
        {
            var nguyenLieuMonAn = _context.NguyenLieuMonAns
                .FirstOrDefault(nl => nl.MonAnId == monAnId && nl.NguyenLieuId == nguyenLieuId);

            if (nguyenLieuMonAn != null)
            {
                _context.NguyenLieuMonAns.Remove(nguyenLieuMonAn);
                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Không tìm thấy nguyên liệu." });
        }

        // Xóa món ăn 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MonAns == null)
            {
                return NotFound();
            }

            var monAn = await _context.MonAns
                .Include(m => m.LoaiMonAn)
                .FirstOrDefaultAsync(m => m.MonAnId == id);
            if (monAn == null)
            {
                return NotFound();
            }

            return View(monAn);
        }

        // POST: MonAns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MonAns == null)
            {
                return Problem("Entity set 'MenuDbContext.MonAns' is null.");
            }

            var monAn = await _context.MonAns.FindAsync(id);
            if (monAn != null)
            {
                // Cập nhật trạng thái món ăn thành false (ngưng bán)
                monAn.TrangThaiMonAn = false;
                _context.MonAns.Update(monAn); // Cập nhật món ăn trong context
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonAnExists(int id)
        {
            return (_context.MonAns?.Any(e => e.MonAnId == id)).GetValueOrDefault();
        }

    }
}
