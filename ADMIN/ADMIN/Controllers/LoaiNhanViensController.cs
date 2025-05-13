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
    public class LoaiNhanViensController : Controller
    {
        private readonly MenuDbContext _context;

        public LoaiNhanViensController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: LoaiNhanViens
        public async Task<IActionResult> Index()
        {
              return _context.LoaiNhanViens != null ? 
                          View(await _context.LoaiNhanViens.ToListAsync()) :
                          Problem("Entity set 'MenuDbContext.LoaiNhanViens'  is null.");
        }

        // GET: LoaiNhanViens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LoaiNhanViens == null)
            {
                return NotFound();
            }

            var loaiNhanVien = await _context.LoaiNhanViens
                .FirstOrDefaultAsync(m => m.LoaiNhanVienId == id);
            if (loaiNhanVien == null)
            {
                return NotFound();
            }

            return View(loaiNhanVien);
        }

        // GET: LoaiNhanViens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoaiNhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoaiNhanVienId,TenLoai")] LoaiNhanVien loaiNhanVien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loaiNhanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiNhanVien);
        }

        // GET: LoaiNhanViens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LoaiNhanViens == null)
            {
                return NotFound();
            }

            var loaiNhanVien = await _context.LoaiNhanViens.FindAsync(id);
            if (loaiNhanVien == null)
            {
                return NotFound();
            }
            return View(loaiNhanVien);
        }

        // POST: LoaiNhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoaiNhanVienId,TenLoai")] LoaiNhanVien loaiNhanVien)
        {
            if (id != loaiNhanVien.LoaiNhanVienId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loaiNhanVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiNhanVienExists(loaiNhanVien.LoaiNhanVienId))
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
            return View(loaiNhanVien);
        }

        // GET: LoaiNhanViens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LoaiNhanViens == null)
            {
                return NotFound();
            }

            var loaiNhanVien = await _context.LoaiNhanViens
                .FirstOrDefaultAsync(m => m.LoaiNhanVienId == id);
            if (loaiNhanVien == null)
            {
                return NotFound();
            }

            return View(loaiNhanVien);
        }

        // POST: LoaiNhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LoaiNhanViens == null)
            {
                return Problem("Entity set 'MenuDbContext.LoaiNhanViens'  is null.");
            }
            var loaiNhanVien = await _context.LoaiNhanViens.FindAsync(id);
            if (loaiNhanVien != null)
            {
                _context.LoaiNhanViens.Remove(loaiNhanVien);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiNhanVienExists(int id)
        {
          return (_context.LoaiNhanViens?.Any(e => e.LoaiNhanVienId == id)).GetValueOrDefault();
        }
    }
}
