using DoAnWeb.Context;
using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DoAnWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;

        public HomeController(ILogger<HomeController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}