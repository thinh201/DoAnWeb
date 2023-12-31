﻿using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Components
{
    [ViewComponent(Name = "CarView")]
    public class CarViewComponent : ViewComponent
    {
        private DataContext _context;
        public CarViewComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var carsWithImages = _context.Cars
                .Where(c => c.IsActive == true)
                .Include(c => c.CarImages)
                .ToList();
           
            return await Task.FromResult((IViewComponentResult)View("Default", carsWithImages));
        }
    }
}
