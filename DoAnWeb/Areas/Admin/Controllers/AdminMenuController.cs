using AspNetCoreHero.ToastNotification.Abstractions;
using DoAnWeb.Context;
using DoAnWeb.Models;
using DoAnWeb.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
     [AdminAuthentication]
    public class AdminMenuController : Controller
    {
        private readonly MyDbContext _context;
        private readonly INotyfService _otyfService;
        public AdminMenuController(MyDbContext context, INotyfService notyfService)
        {
            _context = context;
            _otyfService = notyfService;
        }

        public IActionResult Index()
        {
            var mnList = _context.AdminMenus.OrderBy(m => m.AdminMenuId).ToList();
            return View(mnList);
        }

        public IActionResult Delete(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var mn = _context.AdminMenus.Find(id);

            if (mn == null)
            {
                return NotFound();
            }

            return View(mn);
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var deleMenu = _context.AdminMenus.Find(id);
            if (deleMenu == null)
            {
                return NotFound();
            }

            _context.AdminMenus.Remove(deleMenu);
            _context.SaveChanges();
            _otyfService.Success("Đã xóa menu");
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            var mn = _context.AdminMenus.Where(m => m.ItemLevel == 1).ToList();
            ViewBag.mnList = new SelectList(mn.ToList(), "AdminMenuId", "ItemName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AdminMenu mn)
        {
            if (ModelState.IsValid)
            {
                _context.AdminMenus.Add(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mn);
        }

        public IActionResult Edit(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var item = _context.AdminMenus.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            var mn = _context.AdminMenus.Where(m => m.ItemLevel == 1).ToList();
            ViewBag.mnList = new SelectList(mn.ToList(), "AdminMenuId", "ItemName", item.ParentLevel);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AdminMenu mn)
        {
            if (ModelState.IsValid)
            {
                _context.AdminMenus.Update(mn);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mn);
        }

        public async Task<IActionResult> UpdateActiveStatus(long IdToUpdate)
        {
            if (IdToUpdate == null)
            {
                return new JsonResult(new
                {
                    message = "Error",
                    status = 1
                });
            }

            try
            {
                var ItemById = await _context.AdminMenus.Where(m => m.AdminMenuId == IdToUpdate).FirstOrDefaultAsync();
                if (ItemById == null)
                    return Json(new
                    {
                        status = 2,
                        message = "Cannot find Product"
                    });
                ItemById.IsActive = !ItemById.IsActive;
                _context.AdminMenus.Update(ItemById);
                _context.SaveChanges();
                return Json(new
                {
                    status = 0,
                    currentValue = ItemById.IsActive,
                    message = "Success"
                });
            }
            catch
            {
                return Json(new
                {
                    status = 3,
                    message = "Error from server"
                });
            }
        }
    }
}
