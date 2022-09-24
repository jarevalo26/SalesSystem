using Microsoft.AspNetCore.Mvc;

namespace SalesSystem.Web.Controllers
{
    public class BusinessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
