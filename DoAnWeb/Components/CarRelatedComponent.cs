using DoAnWeb.Context;
using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Components
{
    [ViewComponent(Name = "CarRelated")]
    public class CarRelatedComponent : ViewComponent
    {
        private MyDbContext _context;

        public CarRelatedComponent(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var carsWithImages = await _context.Cars
                .Where(c => c.IsActive == true)
                .OrderByDescending(car => car.CarId)
                .Take(3)
                .ToListAsync();
            return await Task.FromResult((IViewComponentResult)View(carsWithImages));
        }
    }
}