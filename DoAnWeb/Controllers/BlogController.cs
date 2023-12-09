using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
