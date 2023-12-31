﻿using DoAnWeb.Models;
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

        [Route("/car-{slug}-{id:long}.html", Name = "CarDetails")]



        public IActionResult CarDetails(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var carsWithImages = _context.Cars
               .Where(c => c.IsActive == true)
               .Include(c => c.CarImages)
               .ToList();
            if (carsWithImages == null)
            {
                return NotFound();
            }
            return View(carsWithImages);

        }
    }
}
