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
    public class MonAnsController : ControllerBase
    {
        private readonly MenuDbContext _context;

        public MonAnsController(MenuDbContext context)
        {
            _context = context;
        }

        // GET: api/MonAns
        [HttpGet("dsMonan")]
        public async Task<ActionResult<IEnumerable<MonAn>>> dsMonAns()
        {
            if (_context.MonAns == null)
            {
                return NotFound();
            }
            var monAns = await _context.MonAns
                .Where(ma => ma.TrangThaiMonAn == true)
                .Include(ma => ma.LoaiMonAn)
                .Select(ma => new
                {
                    ma.MonAnId,
                    ma.TenMon,
                    Img = Url.Content(ma.Img),
                    ma.Gia,
                    LoaiMonAn = ma.LoaiMonAn.TenLoai,
                })
                .ToListAsync();

            return Ok(monAns);
        }
        [HttpGet("soLuongPhanConLai/{monAnId}")]
        public async Task<ActionResult<int>> SoLuongPhanConLai(int monAnId)
        {
            
            var nguyenLieuMonAns = await _context.NguyenLieuMonAns
                .Where(nlm => nlm.MonAnId == monAnId)  
                .Include(nlm => nlm.NguyenLieu)  
                .ToListAsync();

            
            if (!nguyenLieuMonAns.Any())
            {
                return Ok(100);  
            }

        
            var soLuongPhanConLai = nguyenLieuMonAns
                .Select(nlm => nlm.NguyenLieu.SoLuong / nlm.SoLuong)  
                .Min(); 

            return Ok(soLuongPhanConLai);  
        }
    }
    }
 