using Microsoft.AspNetCore.Mvc;

namespace SalesSystem.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
