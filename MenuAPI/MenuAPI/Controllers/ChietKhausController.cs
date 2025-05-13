using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuAPI.Models;

namespace MenuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChietKhausController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public ChietKhausController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: api/ChietKhaus
        [HttpGet("dschietkhau")]
        public async Task<ActionResult<IEnumerable<ChietKhau>>> GetChietKhaus()
        {
          if (_context.ChietKhaus == null)
          {
              return NotFound();
          }
            var chieckhau = await _context.ChietKhaus.ToListAsync();
            return chieckhau.ToList();
        }
        [HttpPost("giamsoLuong/{maChietKhau}")]
        public async Task<IActionResult> GiamSoLuongChiTietKhau( string maChietKhau)
        {
            if (_context.ChietKhaus == null)
            {
                return NotFound();
            }
            var chietkhau = _context.ChietKhaus.FirstOrDefault(c => c.MaChietKhau == maChietKhau);
            if(chietkhau != null)
            {
                chietkhau.SoLuong -= 1;
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

    }
}
