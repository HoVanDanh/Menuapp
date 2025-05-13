using ADMIN.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ADMIN.Controllers
{
    public class loginController : Controller
    {
        private readonly MenuDbContext _context;

        public loginController(MenuDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            // Kiểm tra thông tin đăng nhập từ cơ sở dữ liệu
            var account = _context.Accounts.FirstOrDefault(a => a.TenDangNhap == username && a.MatKhau == password);

            if (account != null)
            {
                // Kiểm tra phân quyền (Ví dụ: phân quyền là 1 mới có thể đăng nhập)
                if (account.PhanQuyen == 1) 
                {
                    // Lưu tên đăng nhập vào session
                    HttpContext.Session.SetString("Username", account.TenDangNhap);

                    // Chuyển hướng người dùng đến màn hình Home sau khi đăng nhập thành công
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Nếu không có quyền, thông báo lỗi
                    ViewBag.ErrorMessage = "Bạn không có quyền truy cập vào trang này.";
                }
            }
            else
            {
                // Nếu đăng nhập thất bại, hiển thị thông báo lỗi
                ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác!";
            }

            // Trả lại view với thông báo lỗi
            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {
            // Xóa thông tin session của người dùng
            HttpContext.Session.Clear();

            // Chuyển hướng về trang đăng nhập
            return RedirectToAction("Index", "Login");
        }
    }
}
