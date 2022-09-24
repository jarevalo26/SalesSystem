using Microsoft.AspNetCore.Mvc;

namespace SalesSystem.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
