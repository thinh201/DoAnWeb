using DoAnWeb.Context;
using DoAnWeb.Models;
using DoAnWeb.SessionSystem;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Areas.Admin.Component
{
    [ViewComponent(Name = "Header")]
    public class HeaderComponent : ViewComponent
    {
        private readonly MyDbContext _context;
        public HeaderComponent(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var FullName = HttpContext.Session.GetString(SessionKey.FULLNAME);
            var RoleId = HttpContext.Session.GetInt32(SessionKey.ROLEID);
            var UserId = HttpContext.Session.GetInt32(SessionKey.USERID);
            var user = new User();
            if (UserId != null)
            {
                user = _context.Users.Where(m => m.UserId == UserId).FirstOrDefault();
            }
            user.Avatar = "avatar-default.jpg";
            if (RoleId == 1)
            {
                ViewBag.Role = "Quản trị viên";
            } else
            {
                ViewBag.Role = "Người dùng";
            }
            if(FullName != null)
            {
                ViewBag.FullName = FullName;
            }
            ViewBag.UserId = UserId;
            return await Task.FromResult((IViewComponentResult)View("Default", user));
        }
    }
}
