using AspNetCoreHero.ToastNotification.Abstractions;
using DoAnWeb.Context;
using DoAnWeb.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [AdminAuthentication]
    public class OrdersController : Controller
    {
        private readonly MyDbContext _context;
        private readonly INotyfService _otyfService;
        public OrdersController(MyDbContext context, INotyfService notyfService)
        {
            _context = context;
            _otyfService = notyfService;
        }

        // GET
        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            var statuses = _context.Statuses.ToList();
            ViewBag.Statuses = new SelectList(statuses.ToList(), "StatusId", "StatusName");
            return View(orders);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int orderId, int statusId)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null)
            {
                return RedirectToAction("Index");
            }

            order.StatusId = statusId;
            _context.SaveChanges();
            _otyfService.Success("Thành công");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return new JsonResult(new
                {
                    message = "Không tìm thấy đơn hàng này",
                    status = 1
                });
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return new JsonResult(new
            {
                message = "Success",
                status = 0
            });
        }

        [HttpGet]
        [Route("Detail/{id:int}")]
        public IActionResult Detail(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }

            return View(order);
        }
    }
}