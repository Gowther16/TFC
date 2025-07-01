using Microsoft.AspNetCore.Mvc;

namespace TFC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        // Trang quản lý người dùng
        public IActionResult Users()
        {
            ViewBag.Title = "Quản Lý Người Dùng";
            return View();
        }
        // Trang quản lý đơn hàng
        public IActionResult Products()
        {
            ViewBag.Title = "Quản Lý Sản Phẩm";
            return View();
        }
        // Trang quản lý combo
        public IActionResult Combos()
        {
            ViewBag.Title = "Quản Lý Combo";
            return View();
        }
    }
}
