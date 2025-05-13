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
    public class BansController : Controller
    {
        private readonly MenuDbContext _context;

        public BansController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: Bans
        public async Task<IActionResult> Index()
        {
              return _context.Bans != null ? 
                          View(await _context.Bans.ToListAsync()) :
                          Problem("Entity set 'MenuDbContext.Bans'  is null.");
        }

        // GET: Bans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bans == null)
            {
                return NotFound();
            }

            var ban = await _context.Bans
                .FirstOrDefaultAsync(m => m.BanId == id);
            if (ban == null)
            {
                return NotFound();
            }

            return View(ban);
        }

        // GET: Bans/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BanId,TenBan,TrangThai")] Ban ban)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ban);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ban);
        }

        // GET: Bans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bans == null)
            {
                return NotFound();
            }

            var ban = await _context.Bans.FindAsync(id);
            if (ban == null)
            {
                return NotFound();
            }
            return View(ban);
        }

        // POST: Bans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BanId,TenBan,TrangThai")] Ban ban)
        {
            if (id != ban.BanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ban.TrangThai)
                    {
                        ModelState.AddModelError(string.Empty, "Không thể sửa bàn đang sử dụng.");
                        return View(ban);
                    }
                    _context.Update(ban);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BanExists(ban.BanId))
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
            return View(ban);
        }

        // GET: Bans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bans == null)
            {
                return NotFound();
            }

            var ban = await _context.Bans
                .FirstOrDefaultAsync(m => m.BanId == id);
            if (ban == null)
            {
                return NotFound();
            }

            return View(ban);
        }

        // POST: Bans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bans == null)
            {
                return Problem("Entity set 'MenuDbContext.Bans'  is null.");
            }
            var ban = await _context.Bans.FindAsync(id);
            if (ban != null)
            {
                _context.Bans.Remove(ban);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BanExists(int id)
        {
          return (_context.Bans?.Any(e => e.BanId == id)).GetValueOrDefault();
        }
    }
}
