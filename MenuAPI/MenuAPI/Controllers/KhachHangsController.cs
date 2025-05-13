using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPI.Models;
using MenuAPI.DTO;

namespace MenuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangsController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public KhachHangsController(MenuDbContext context)
        {
            _context = context;
        }

        [HttpGet("dskhachhang")]
        public async Task<ActionResult<IEnumerable<KhachHangDTO>>> GetKhachHangs()
        {
            if (_context.KhachHangs == null)
            {
                return NotFound();
            }

            var khachHangs = await _context.KhachHangs
                .Select(kh => new KhachHangDTO
                {
                    KhachHangId = kh.KhachHangId,
                    TenKhachHang = kh.TenKhachHang,
                    SoDienThoai = kh.SoDienThoai,
                    DiaChi = kh.DiaChi
                })
                .ToListAsync();

            return Ok(khachHangs);
        }
        //// GET: api/KhachHangs/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<KhachHang>> GetKhachHang(int id)
        //{
        //  if (_context.KhachHangs == null)
        //  {
        //      return NotFound();
        //  }
        //    var khachHang = await _context.KhachHangs.FindAsync(id);

        //    if (khachHang == null)
        //    {
        //        return NotFound();
        //    }

        //    return khachHang;
        //}

        //// PUT: api/KhachHangs/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutKhachHang(int id, KhachHang khachHang)
        //{
        //    if (id != khachHang.KhachHangId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(khachHang).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!KhachHangExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/KhachHangs
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<KhachHang>> PostKhachHang(KhachHang khachHang)
        //{
        //  if (_context.KhachHangs == null)
        //  {
        //      return Problem("Entity set 'MenuDbContext.KhachHangs'  is null.");
        //  }
        //    _context.KhachHangs.Add(khachHang);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetKhachHang", new { id = khachHang.KhachHangId }, khachHang);
        //}

        //// DELETE: api/KhachHangs/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteKhachHang(int id)
        //{
        //    if (_context.KhachHangs == null)
        //    {
        //        return NotFound();
        //    }
        //    var khachHang = await _context.KhachHangs.FindAsync(id);
        //    if (khachHang == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.KhachHangs.Remove(khachHang);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool KhachHangExists(int id)
        {
            return (_context.KhachHangs?.Any(e => e.KhachHangId == id)).GetValueOrDefault();
        }
    }
}
