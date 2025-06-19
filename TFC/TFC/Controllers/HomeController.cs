using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFC.Models;

namespace TFC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        public IActionResult AdminOnly()
        {
            return View();
        }

        [Authorize(Roles = "admin,staff")]
        public IActionResult StaffAndAdmin()
        {
            return View();
        }

    }
}
