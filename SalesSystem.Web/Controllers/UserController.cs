using Microsoft.AspNetCore.Mvc;

namespace SalesSystem.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
