using DoAnWeb.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Controllers
{
    [Route("[controller]")]
    public class CarController : Controller
    {
        private readonly MyDbContext _context;

        public CarController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var carsWithImages = _context.Cars
                .Where(c => c.IsActive == true)
                .ToList();
            return View(carsWithImages);
        }

        [HttpGet]
        [Route("{id}/{slug}")]
        public IActionResult Detail(int id, string? slug)
        {
            var car = _context.Cars.Where(car => car.CarId == id && car.IsActive == true)
                .SingleOrDefault();
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
    }
}