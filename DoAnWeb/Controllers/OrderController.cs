using AspNetCoreHero.ToastNotification.Abstractions;
using DoAnWeb.Config;
using DoAnWeb.Context;
using DoAnWeb.Models;
using DoAnWeb.Models.Authentication;
using DoAnWeb.Models.ViewModel;
using DoAnWeb.SessionSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAnWeb.Controllers
{
    [Route("[controller]")]
    [CustomerAuthentication]
    public class OrderController : Controller
    {
        private readonly MyDbContext _context;
        private readonly INotyfService _otyfService;
        public OrderController(MyDbContext context, INotyfService notyfService)
        {
            _context = context;
            _otyfService = notyfService;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(CreateOrderModel model)
        {
            var userId = HttpContext.Session.GetInt32(SessionKey.USERID);
            if (userId == null)
            {
                _otyfService.Warning("Vui lòng đăng nhập trước");
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                _otyfService.Warning("Vui lòng đăng nhập trước");
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var car = _context.Cars.FirstOrDefault(c => c.CarId == model.CarId);
                if (car == null)
                {
                    _otyfService.Error("Đặt xe thất bại, không tìm thấy xe");
                    return RedirectToAction("Index", "Home");
                }

                int bookingDays = GetDays(model.StartDate ?? DateTime.Now, model.EndDate ?? DateTime.Now.AddDays(1));
                decimal? totalFee = bookingDays * car.PricePerDay * model.Quantity;

                var order = new Order
                {
                    CarId = model.CarId,
                    CustomerId = user.UserId,
                    Quantity = model.Quantity,
                    FullName = model.UseDefaultInfo ? user.FullName : model.FullName,
                    Address = model.UseDefaultInfo ? user.Address : model.Address,
                    PhoneNumber = model.UseDefaultInfo ? user.Phone : model.PhoneNumber,
                    Email = model.UseDefaultInfo ? user.Email : model.Email,
                    StatusId = OrderStatus.Approved,
                    TotalFee = totalFee,
                    BookingDays = bookingDays,
                    OrderDate = DateTime.Now,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                _otyfService.Success("Đặt xe thành công");
                return RedirectToAction("Orders", "Account");
            }
            _otyfService.Error("Đặt xe thất bại");
            return RedirectToAction("Index", "Home");
        }

        private static int GetDays(DateTime start, DateTime end)
        {
            return (int)(end - start).TotalDays;
        }

        [HttpPost]
        [Route("Cancel")]
        public IActionResult Cancel(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return Ok(new
                {
                    message = "Hủy đơn hàng thất bại",
                    code = 400
                });
            }
            if (order.StatusId == OrderStatus.Cancelled)
            {
                return Ok(new
                {
                    message = "Đơn hàng đã được hủy trước đó",
                    code = 400
                });
            }
            if (order.StatusId > OrderStatus.Approved)
            {
                return Ok(new
                {
                    message = "Đơn hàng đã được xác nhận, không thể hủy",
                    code = 400
                });
            }

            order.StatusId = OrderStatus.Cancelled;
            _context.Orders.Update(order);
            _context.SaveChanges();
            return Ok(new
            {
                message = "Hủy đơn hàng thành công",
                code = 200
            });

        }
    }
}