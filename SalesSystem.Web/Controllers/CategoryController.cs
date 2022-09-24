using Microsoft.AspNetCore.Mvc;

namespace SalesSystem.Web.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
