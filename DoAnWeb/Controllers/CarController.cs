using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Controllers
{
    public class CarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
