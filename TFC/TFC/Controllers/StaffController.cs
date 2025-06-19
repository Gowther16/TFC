using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TFC.Controllers
{
    [Authorize(Roles = "staff,admin")]
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Trang đơn hàng mới
        public IActionResult Orders()
        {
            ViewBag.Title = "Đơn Hàng Mới";
            return View();
        }

        // Trang kho hàng
        public IActionResult Kitchen()
        {
            ViewBag.Title = "Kho Hàng";
            return View();
        }
    }
}

