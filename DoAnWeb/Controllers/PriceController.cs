using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Context;

namespace DoAnWeb.Controllers
{
    public class PriceController : Controller
    {
        private readonly MyDbContext _context;

        public PriceController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cars = _context.Cars.Where(car => car.IsActive == true).ToList();
            return View(cars);
        }
    }
}