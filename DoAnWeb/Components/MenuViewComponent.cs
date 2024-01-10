using Microsoft.AspNetCore.Mvc;
using DoAnWeb.Models;
using DoAnWeb.Context;
using DoAnWeb.SessionSystem;

namespace DoAnWeb.Components
{
    [ViewComponent(Name = "MenuView")]
    public class MenuViewComponent : ViewComponent
    {
        private MyDbContext _context;
        public MenuViewComponent(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var FullName = HttpContext.Session.GetString(SessionKey.FULLNAME);
            var UserId = HttpContext.Session.GetInt32(SessionKey.USERID);
            var listofMenu = (from m in _context.Menus
                              where (m.IsActive == true) && (m.Position == 1)
                              select m).ToList();
            ViewBag.FullName = FullName; ViewBag.UserId = UserId; ViewBag.RoleId = HttpContext.Session.GetInt32(SessionKey.ROLEID);
            return await Task.FromResult((IViewComponentResult)View("Default", listofMenu));
        }
    }
}
