using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFC.Models;

namespace TFC.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            ModelContext modelContext = new ModelContext();
            var categories = modelContext.Categories
            .Include(c => c.Products.OrderBy(p => p.Id))
            .OrderBy(c => c.Id)
            .ToList();

            var combos = modelContext.Combos.ToList();
            ViewBag.Combos = combos;
            ViewBag.Categories = categories;
            return View();
        }
    }
}
