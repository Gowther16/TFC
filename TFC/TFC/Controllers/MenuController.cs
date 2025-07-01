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

            var combos = modelContext.Combos
                .Include(c => c.Comboitems)
                .ThenInclude(ci => ci.Product)
                .ToList();

            // Check combo availability based on product inventory
            foreach (var combo in combos)
            {
                combo.Available = CheckComboAvailability(combo) ? true : false;
            }

            ViewBag.Combos = combos;
            ViewBag.Categories = categories;
            return View();
        }

        private bool CheckComboAvailability(Combo combo)
        {
            if (combo.Comboitems == null || !combo.Comboitems.Any())
                return false;

            foreach (var comboItem in combo.Comboitems)
            {
                if (comboItem.Product == null ||
                    comboItem.Product.Inventory == null ||
                    comboItem.Product.Inventory <= 0 || 
                    comboItem.Product.Inventory < comboItem.Quantity)
                {
                    return false;
                }
            }
            return true;
        }
    }
}