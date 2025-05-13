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
    public class NguyenLieusController : Controller
    {
        private readonly MenuDbContext _context;

        public NguyenLieusController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: NguyenLieus
        public async Task<IActionResult> Index(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var ingredients = _context.NguyenLieus.AsQueryable();

            // Search logic
            if (!string.IsNullOrEmpty(searchString))
            {
                ingredients = ingredients.Where(nl => nl.TenNguyenLieu.Contains(searchString));
            }

            var totalIngredients = await ingredients.CountAsync();

            // Pagination logic
            var ingredientList = await ingredients
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalIngredients / (double)pageSize);

            // Set ViewBag values
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;

            return View(ingredientList);
        }

        // GET: NguyenLieus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NguyenLieus == null)
            {
                return NotFound();
            }

            var nguyenLieu = await _context.NguyenLieus.FirstOrDefaultAsync(m => m.NguyenLieuId == id);
            if (nguyenLieu == null)
            {
                return NotFound();
            }

            return View(nguyenLieu);
        }

        // GET: NguyenLieus/Create
        public IActionResult Create()
        {
            ViewBag.DanhSachNhaCungCap = new SelectList(_context.NhaCungCaps, "NhaCungCapId", "TenNhaCungCap");
            return View();
        }

        // POST: NguyenLieus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NguyenLieuId,TenNguyenLieu,SoLuong,DonVi,NhaCungCapId")] NguyenLieu nguyenLieu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nguyenLieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DanhSachNhaCungCap = new SelectList(_context.NhaCungCaps, "NhaCungCapId", "TenNhaCungCap");
            return View(nguyenLieu);
        }

        // GET: NguyenLieus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NguyenLieus == null)
            {
                return NotFound();
            }

            var nguyenLieu = await _context.NguyenLieus.FindAsync(id);
            if (nguyenLieu == null)
            {
                return NotFound();
            }
            return View(nguyenLieu);
        }

        // POST: NguyenLieus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NguyenLieuId,TenNguyenLieu,SoLuong,DonVi,NhaCungCapId")] NguyenLieu nguyenLieu)
        {
            if (id != nguyenLieu.NguyenLieuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nguyenLieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguyenLieuExists(nguyenLieu.NguyenLieuId))
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
            return View(nguyenLieu);
        }

        // GET: NguyenLieus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NguyenLieus == null)
            {
                return NotFound();
            }

            var nguyenLieu = await _context.NguyenLieus.FirstOrDefaultAsync(m => m.NguyenLieuId == id);
            if (nguyenLieu == null)
            {
                return NotFound();
            }

            return View(nguyenLieu);
        }

        // POST: NguyenLieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NguyenLieus == null)
            {
                return Problem("Entity set 'MenuDbContext.NguyenLieus' is null.");
            }
            var nguyenLieu = await _context.NguyenLieus.FindAsync(id);
            if (nguyenLieu != null)
            {
                _context.NguyenLieus.Remove(nguyenLieu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NguyenLieuExists(int id)
        {
            return (_context.NguyenLieus?.Any(e => e.NguyenLieuId == id)).GetValueOrDefault();
        }

        [HttpPost]
        public JsonResult CreateNguyenLieu([FromBody] NguyenLieu model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.NguyenLieus.Add(model);
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Thông tin không hợp lệ." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}
