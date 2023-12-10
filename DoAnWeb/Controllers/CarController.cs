using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Controllers
{
    public class CarController : Controller
    {
        private readonly DataContext _context;

        public CarController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var carsWithImages = _context.Cars
                .Where(c => c.IsActive == true)
                .Include(c => c.CarImages)
                .ToList();

            return View(carsWithImages);
        }
    }
}
