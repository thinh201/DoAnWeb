using AspNetCoreHero.ToastNotification.Abstractions;
using DoAnWeb.Context;
using DoAnWeb.Models;
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
    // [AdminAuthentication]
    public class MenuController : Controller
    {
        private readonly MyDbContext _context;
        private readonly INotyfService _otyfService;
        public MenuController(MyDbContext context, INotyfService notyfService)
        {
            _context = context;
            _otyfService = notyfService;
        }

        public IActionResult Index()
        {
            var mnList = _context.Menus.OrderBy(m => m.MenuId).ToList();
            return View(mnList);
        }

        public IActionResult Delete(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var mn = _context.Menus.Find(id);

            if (mn == null)
            {
                return NotFound();
            }

            return View(mn);
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var deleMenu = _context.Menus.Find(id);
            if (deleMenu == null)
            {
                return NotFound();
            }

            _context.Menus.Remove(deleMenu);
            _context.SaveChanges();
            _otyfService.Success("Đã xóa menu");
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            var mnList = (from m in _context.Menus
                select new SelectListItem()
                {
                    Text = m.MenuName,
                    Value = m.MenuId.ToString(),
                }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = "0"
            });
            ViewBag.mnList = mnList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Menu mn)
        {
            if (ModelState.IsValid)
            {
                _context.Menus.Add(mn);
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

            var mn = _context.Menus.Find(id);
            if (mn == null)
            {
                return NotFound();
            }

            var mnList = (from m in _context.Menus
                select new SelectListItem()
                {
                    Text = m.MenuName,
                    Value = m.MenuId.ToString(),
                }).ToList();
            mnList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            ViewBag.mnList = mnList;
            _otyfService.Success("Sửa thành công");
            return View(mn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Menu mn)
        {
            if (ModelState.IsValid)
            {
                _context.Menus.Update(mn);
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
                var ItemById = await _context.Menus.Where(m => m.MenuId == IdToUpdate).FirstOrDefaultAsync();
                if (ItemById == null)
                    return Json(new
                    {
                        status = 2,
                        message = "Cannot find Product"
                    });
                ItemById.IsActive = !ItemById.IsActive;
                _context.Menus.Update(ItemById);
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