using Microsoft.AspNetCore.Mvc;

namespace SalesSystem.Web.Controllers
{
    public class SaleController : Controller
    {
        public IActionResult NewSale()
        {
            return View();
        }

        public IActionResult HistorySale()
        {
            return View();
        }
    }
}
