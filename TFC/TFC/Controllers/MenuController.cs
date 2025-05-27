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
            .Include(c => c.Products)
            .OrderBy(c => c.Id) 
            .ToList();
            foreach (var category in categories)
            {
                category.Products = category.Products
                    .OrderBy(p => p.Id)
                    .ToList();
            }

            return View(categories);
        }
    }
}
