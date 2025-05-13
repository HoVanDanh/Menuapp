using Microsoft.AspNetCore.Mvc;

namespace ADMIN.Controllers
{
    public class ThongKesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
