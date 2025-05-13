using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ADMIN.Repository;
using ADMIN.Repository.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ADMIN.Controllers
{
    public class NhaCungCapsController : Controller
    {
        private readonly MenuDbContext _context;

        public NhaCungCapsController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: NhaCungCaps
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var suppliers = _context.NhaCungCaps.Where(s => s.TrangThai == true).AsQueryable();

            // Tìm kiếm theo tên nhà cung cấp
            if (!string.IsNullOrEmpty(searchString))
            {
                suppliers = suppliers.Where(s => s.TenNhaCungCap.Contains(searchString));
            }

            // Tổng số nhà cung cấp
            var totalSuppliers = await suppliers.CountAsync();

            // Lấy dữ liệu phân trang
            var supplierList = await suppliers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Tính toán số trang
            var totalPages = (int)Math.Ceiling(totalSuppliers / (double)pageSize);

            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;

            return View(supplierList);
        }

        // GET: NhaCungCaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhaCungCaps == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _context.NhaCungCaps
                .FirstOrDefaultAsync(m => m.NhaCungCapId == id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Create
        public IActionResult Create()
        {
            return View(new NhaCungCap());
        }

        // POST: NhaCungCaps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhaCungCap nhaCungCap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhaCungCap);
                await _context.SaveChangesAsync();

                // Kiểm tra nếu yêu cầu là AJAX
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Tạo nhà cung cấp thành công!", data = nhaCungCap });
                }

                return RedirectToAction(nameof(Index));
            }

            // Trả về thông tin lỗi nếu có
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            return View(nhaCungCap);
        }


        // GET: NhaCungCaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NhaCungCaps == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }
            return View(nhaCungCap);
        }

        // POST: NhaCungCaps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NhaCungCapId,TenNhaCungCap,DiaChi,SoDienThoai")] NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.NhaCungCapId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaCungCap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaCungCapExists(nhaCungCap.NhaCungCapId))
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
            return View(nhaCungCap);
        }

        // GET: NhaCungCaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NhaCungCaps == null)
            {
                return NotFound();
            }

            var nhaCungCap = await _context.NhaCungCaps
                .FirstOrDefaultAsync(m => m.NhaCungCapId == id);
            if (nhaCungCap == null)
            {
                return NotFound();
            }

            return View(nhaCungCap);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NhaCungCaps == null)
            {
                return Problem("Entity set 'MenuDbContext.NhaCungCaps' is null.");
            }

            // Tìm nhà cung cấp theo id
            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            if (nhaCungCap != null)
            {
                nhaCungCap.TrangThai = false;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool NhaCungCapExists(int id)
        {
            return _context.NhaCungCaps.Any(e => e.NhaCungCapId == id);
        }
    }
}
