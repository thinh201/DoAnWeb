using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Controllers
{
    public class ErrorController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ErrorRole()
        {
            return View();
        }
    }
}
