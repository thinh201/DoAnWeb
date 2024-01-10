using DoAnWeb.Context;
using DoAnWeb.Models;
using DoAnWeb.SessionSystem;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Areas.Admin.Component
{
    [ViewComponent(Name = "Navbar")]
    public class NavbarComponent : ViewComponent
    {
        private readonly MyDbContext _context;
        public NavbarComponent(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var listMenu = _context.AdminMenus.ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listMenu));
        }
    }
}
