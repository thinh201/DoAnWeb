using DoAnWeb.Models.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
     [AdminAuthentication]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}