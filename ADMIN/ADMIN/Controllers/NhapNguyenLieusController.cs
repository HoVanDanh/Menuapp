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
    public class NhapNguyenLieusController : Controller
    {
        private readonly MenuDbContext _context;

        public NhapNguyenLieusController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: NhapNguyenLieus
        public async Task<IActionResult> Index(int? nhaCungCapId)
        {
            var nhapNguyenLieuQuery = _context.NhapNguyenLieus
                                              .Include(n => n.NguyenLieu)
                                              .Include(n => n.NhaCungCap)
                                              .AsQueryable();

            if (nhaCungCapId.HasValue)
            {
                nhapNguyenLieuQuery = nhapNguyenLieuQuery.Where(n => n.NhaCungCapId == nhaCungCapId.Value);
            }

            ViewBag.DanhSachNhaCungCap = new SelectList(_context.NhaCungCaps, "NhaCungCapId", "TenNhaCungCap");
            ViewBag.SelectedNhaCungCapId = nhaCungCapId;

            var nhapNguyenLieuList = await nhapNguyenLieuQuery.OrderByDescending(n => n.NgayNhap).ToListAsync();
            return View(nhapNguyenLieuList);
        }


        // GET: NhapNguyenLieus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhapNguyenLieus == null)
            {
                return NotFound();
            }

            var nhapNguyenLieu = await _context.NhapNguyenLieus
                .Include(n => n.NguyenLieu)
                .Include(n => n.NhaCungCap)
                .FirstOrDefaultAsync(m => m.NhapNguyenLieuId == id);
            if (nhapNguyenLieu == null)
            {
                return NotFound();
            }
            ViewBag.DanhSachNguyenLieu = new SelectList(_context.NguyenLieus, "NguyenLieuId", "TenNguyenLieu");
            ViewBag.DanhSachNhaCungCap = new SelectList(_context.NhaCungCaps, "NhaCungCapId", "TenNhaCungCap");
            return View(nhapNguyenLieu);
        }

        // GET: NhapNguyenLieus/Create
        public IActionResult Create()
        {
            ViewBag.DanhSachNguyenLieu = new SelectList(_context.NguyenLieus, "NguyenLieuId", "TenNguyenLieu");
            ViewBag.DanhSachNhaCungCap = new SelectList(_context.NhaCungCaps, "NhaCungCapId", "TenNhaCungCap");
            return View();
        }

        // POST: NhapNguyenLieus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NhapNguyenLieuId,NguyenLieuId,NhaCungCapId,SoLuong")] NhapNguyenLieu nhapNguyenLieu)
        {

            nhapNguyenLieu.NgayNhap = DateTime.Now; // Gán thời điểm hiện tại làm ngày nhập
            var nguyenlieu = await _context.NguyenLieus.FirstOrDefaultAsync(nl => nl.NguyenLieuId == nhapNguyenLieu.NguyenLieuId);
            if (nguyenlieu != null)
            {
                nguyenlieu.SoLuong += nhapNguyenLieu.SoLuong;
                _context.Update(nguyenlieu);
            }
            _context.Add(nhapNguyenLieu);

            Console.WriteLine("Dữ liệu được thêm: " + nhapNguyenLieu);
            await _context.SaveChangesAsync();
            ViewBag.DanhSachNguyenLieu = new SelectList(_context.NguyenLieus, "NguyenLieuId", "TenNguyenLieu", nhapNguyenLieu.NguyenLieuId);
            ViewBag.DanhSachNhaCungCap = new SelectList(_context.NhaCungCaps, "NhaCungCapId", "TenNhaCungCap", nhapNguyenLieu.NhaCungCapId);
            return RedirectToAction(nameof(Index));

        }

        // GET: NhapNguyenLieus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NhapNguyenLieus == null)
            {
                return NotFound();
            }

            var nhapNguyenLieu = await _context.NhapNguyenLieus.FindAsync(id);
            if (nhapNguyenLieu == null)
            {
                return NotFound();
            }
            ViewBag.DanhSachNguyenLieu = new SelectList(_context.NguyenLieus, "NguyenLieuId", "TenNguyenLieu", nhapNguyenLieu.NguyenLieuId);
            ViewBag.DanhSachNhaCungCap = new SelectList(_context.NhaCungCaps, "NhaCungCapId", "TenNhaCungCap", nhapNguyenLieu.NhaCungCapId);
            return View(nhapNguyenLieu);
        }

        // POST: NhapNguyenLieus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NhapNguyenLieuId,NguyenLieuId,NhaCungCapId,SoLuong,NgayNhap")] NhapNguyenLieu nhapNguyenLieu)
        {
            if (id != nhapNguyenLieu.NhapNguyenLieuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhapNguyenLieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhapNguyenLieuExists(nhapNguyenLieu.NhapNguyenLieuId))
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
            ViewBag.DanhSachNguyenLieu = new SelectList(_context.NguyenLieus, "NguyenLieuId", "TenNguyenLieu", nhapNguyenLieu.NguyenLieuId);
            ViewBag.DanhSachNhaCungCap = new SelectList(_context.NhaCungCaps, "NhaCungCapId", "TenNhaCungCap", nhapNguyenLieu.NhaCungCapId);
            return View(nhapNguyenLieu);
        }

        // GET: NhapNguyenLieus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NhapNguyenLieus == null)
            {
                return NotFound();
            }

            var nhapNguyenLieu = await _context.NhapNguyenLieus
                .Include(n => n.NguyenLieu)
                .Include(n => n.NhaCungCap)
                .FirstOrDefaultAsync(m => m.NhapNguyenLieuId == id);
            if (nhapNguyenLieu == null)
            {
                return NotFound();
            }

            return View(nhapNguyenLieu);
        }

        // POST: NhapNguyenLieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NhapNguyenLieus == null)
            {
                return Problem("Entity set 'MenuDbContext.NhapNguyenLieus' is null.");
            }
            var nhapNguyenLieu = await _context.NhapNguyenLieus.FindAsync(id);
            if (nhapNguyenLieu != null)
            {
                _context.NhapNguyenLieus.Remove(nhapNguyenLieu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhapNguyenLieuExists(int id)
        {
            return (_context.NhapNguyenLieus?.Any(e => e.NhapNguyenLieuId == id)).GetValueOrDefault();
        }
    }
}
