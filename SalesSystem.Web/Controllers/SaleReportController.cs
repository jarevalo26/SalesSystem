using Microsoft.AspNetCore.Mvc;

namespace SalesSystem.Web.Controllers
{
    public class SaleReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
