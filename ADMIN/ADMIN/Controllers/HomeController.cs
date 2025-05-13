using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ADMIN.Repository;
using System.Linq;
using ADMIN.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ADMIN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MenuDbContext _context;

        public HomeController(ILogger<HomeController> logger, MenuDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    ViewBag.DanhSachHoaDon = _context.HoaDons
        //        .Include(h => h.Ban)
        //        .Include(h => h.KhachHang)
        //        .ToList();

        //    return View();
        //}
        public async Task<IActionResult> Index()
        {
            // Lấy ngày hiện tại
            var today = DateTime.Today;

            // Lọc hóa đơn theo ngày hiện tại và tính toán số lượng đơn hàng và tổng doanh thu
            var danhSachHoaDonHomNay = await _context.HoaDons
                .Where(h => h.NgayTao.Date == today)
                .ToListAsync();

            int soLuongDonHang = danhSachHoaDonHomNay.Count;
            decimal tongDoanhThu = danhSachHoaDonHomNay.Sum(h => h.TongTien);

            // Truyền dữ liệu qua ViewBag
            ViewBag.SoLuongDonHang = soLuongDonHang;
            ViewBag.TongDoanhThu = tongDoanhThu;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
