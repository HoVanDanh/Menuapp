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
    public class BansController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public BansController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: api/Bans
        [HttpGet("dsBans")]
        public async Task<ActionResult<IEnumerable<Ban>>> GetBans()
        {
          if (_context.Bans == null)
          {
              return NotFound();
          }
            var DsBan = await _context.Bans.Select(
                ban => new
                {
                    ban.BanId,
                    ban.TenBan,
                    ban.TrangThai,
                }).ToListAsync();
            return Ok(DsBan);
            //var dsBan = await _context.Bans.Select(ban => new
            //{
            //    ban.BanId,
            //    ban.TenBan,
            //    ban.TrangThai,
            //    ThoiGianNgoi = ban.TrangThai == true
            //    ? _context.ChiTietDatBans
            //        .Where(ct => ct.BanId == ban.BanId)
            //        .OrderBy(ct => ct.ThoiGianTao)
            //        .Select(ct => ct.ThoiGianTao)
            //        .FirstOrDefault()
            //    : (DateTime?)null,
            //}).ToListAsync();

            //// Tính thời gian ngồi nếu có khách
            //var result = dsBan.Select(ban => new
            //{
            //    ban.BanId,
            //    ban.TenBan,
            //    ban.TrangThai,
            //    ThoiGianNgoi = ban.ThoiGianNgoi != null
            //        ? (int)(DateTime.Now - ban.ThoiGianNgoi.Value).TotalMinutes 
            //        : 0
            //}).ToList();

            //return Ok(result);
        }
        [HttpPost("TrangThaiban")]
        public async Task<ActionResult> UpdateTrangThaiBan(updateban req)
        {
            var ban= await _context.Bans.FindAsync(req.BanId);
            if(ban == null)
            {
                return NotFound();
            }
            ban.TrangThai=  req.TrangThai;
            await _context.SaveChangesAsync();
            return Ok(ban);
        }


        private bool BanExists(int id)
        {
            return (_context.Bans?.Any(e => e.BanId == id)).GetValueOrDefault();
        }
    }
}
