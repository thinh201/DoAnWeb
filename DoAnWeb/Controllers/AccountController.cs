using DoAnWeb.Context;
using DoAnWeb.Extension;
using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;
using DoAnWeb.SessionSystem;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using DoAnWeb.Ultilities;
using DoAnWeb.Utilities;
using DoAnWeb.Models.Authentication;

namespace DoAnWeb.Controllers
{

    public class AccountController : Controller
    {
        private readonly MyDbContext _context;
        private readonly INotyfService _notyfService;

        public AccountController(MyDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public IActionResult Login()
        {
            var userId = HttpContext.Session.GetInt32(SessionKey.USERID);
            if (userId != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                string password = HashPassword.MD5Password(user.Password);
                var CheckLogin = _context.Users
                    .Where(m => m.Email.ToLower() == user.Email.ToLower() && m.Password == password).FirstOrDefault();
                if (CheckLogin != null)
                {
                    HttpContext.Session.SetString(SessionKey.FULLNAME, CheckLogin.FullName);
                    HttpContext.Session.SetString(SessionKey.EMAIL, CheckLogin.Email);
                    HttpContext.Session.SetInt32(SessionKey.USERID, Convert.ToInt32(CheckLogin.UserId));
                    HttpContext.Session.SetInt32(SessionKey.ROLEID, (int)CheckLogin.RoleId);
                    _notyfService.Success("Đăng nhập thành công");
                    if (CheckLogin.RoleId == 1)
                    {
                        return Redirect("/Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    _notyfService.Error("Đăng nhập thất bại");
                    TempData["LoginError"] = "Tài khoản hoặc mật khẩu không chính xác";
                    return View();
                }
            }
            catch
            {
                return NotFound();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove(SessionKey.FULLNAME);
            HttpContext.Session.Remove(SessionKey.ROLEID);
            HttpContext.Session.Remove(SessionKey.USERID);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user, string AgainPass)
        {
            if (user == null)
            {
                _notyfService.Error("Server đang bị lỗi");
                return NotFound();
            }

            try
            {
                var CheckEmail = _context.Users.Where(m => m.Email == user.Email).FirstOrDefault();
                if (user.Email == null)
                {
                    TempData["EmailRequired"] = "Vui lòng nhập Email của bạn";
                }

                if (user.FullName == null)
                {
                    TempData["FullNameRequired"] = "Vui lòng nhập họ và tên của bạn";
                }

                if (user.Password == null)
                {
                    TempData["PasswordRequired"] = "Vui lòng nhập mật khẩu của bạn";
                }

                if (user.Address == null)
                {
                    TempData["AddressRequired"] = "Vui lòng nhập Địa chỉ của bạn";
                }

                if (user.Phone == null)
                {
                    TempData["PhoneNumberRequired"] = "Vui lòng nhập điện thoại của bạn";
                }

                if (AgainPass == null)
                {
                    TempData["AgainPassRequired"] = "Trường này không được để trống";
                }

                if (user.Email != null && CheckEmail != null)
                {
                    TempData["EmailExists"] = "Email này đã được sử dụng";
                }

                if (user.Password != null && AgainPass != null && user.Password.Trim() != AgainPass.Trim())
                {
                    TempData["AgainPassError"] = "Mật khẩu nhập lại không trùng khớp";
                }

                if (user.Email == null || CheckEmail != null || user.FullName == null || user.Password == null ||
                    user.Address == null || user.Phone == null || AgainPass == null ||
                    user.Password.Trim() != AgainPass.Trim())
                {
                    _notyfService.Error("Vui lòng nhập đầy đủ các thông tin");
                    return View(user);
                }

                user.IsBlocked = false;
                user.RoleId = 2;
                user.Avatar = "avatar-default.jpg";
                user.Password = HashPassword.MD5Password(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                _notyfService.Success("Đăng kí tài khoản thành công");
                return RedirectToAction("Login");
            }
            catch
            {
                _notyfService.Error("Server đang bị lỗi");
                return NotFound();
            }
        }
        [CustomerAuthentication]
        public IActionResult Profile()
        {
            var UserId = HttpContext.Session.GetInt32(SessionKey.USERID);
            var user = _context.Users.AsNoTracking().Where(m => m.UserId == Convert.ToInt64(UserId))
                .Include(m => m.Role).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }
        [CustomerAuthentication]
        [HttpPost]
        public IActionResult UpdateProfile(User user, IFormFile? avatar)
        {
            var userId = HttpContext.Session.GetInt32(SessionKey.USERID);
            if (user.UserId != Convert.ToInt64(userId))
            {
                _notyfService.Error("Bạn không có quyền thay đổi thông tin của người khác");
                return Redirect("/");
            }

            try
            {
                var userUpdate = _context.Users.Where(m => m.UserId == Convert.ToInt64(userId))
                    .SingleOrDefault();
                if (userUpdate == null) return NotFound();
                if (user.FullName == null) TempData["FullName"] = "Họ và tên khong được để trống";
                if (user.Email == null) TempData["Email"] = "Email không được để trống";
                if (user.Phone == null) TempData["Phone"] = "Điện thoại không được để trống";
                if (user.Address == null) TempData["Address"] = "Địa chỉ không được để trống";
                if (user.FullName == null || user.Email == null || user.Address == null || user.Phone == null)
                {
                    _notyfService.Error("Phải điền đầy đủ thông tin");
                    return View("Profile", user);
                }

                userUpdate.FullName = user.FullName;
                userUpdate.Email = user.Email;
                userUpdate.Phone = user.Phone;
                userUpdate.Address = user.Address;
                userUpdate.Avatar = avatar != null ? UploadImage.UploadSingleImage(avatar) : userUpdate.Avatar;
                _context.Users.Update(userUpdate);
                _context.SaveChanges();
                _notyfService.Success("Cập nhật thông tin thành công");
                return RedirectToAction("Profile");
            }
            catch
            {
                _notyfService.Error("Lỗi mất rồi");
                return View("Profile");
            }
        }
        [HttpGet]
        [CustomerAuthentication]
        [Route("Account/Orders")]
        public IActionResult Orders()
        {
            var userId = HttpContext.Session.GetInt32(SessionKey.USERID);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var orders = _context.Orders.OrderByDescending(m => m.BookingDays).Where(m => m.CustomerId == Convert.ToInt64(userId)).ToList();
            return View(orders);
        }
        [CustomerAuthentication]
        [HttpGet]
        [Route("Account/Order-Detail/{id:int}")]
        public IActionResult OrderDetail(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return RedirectToAction("Orders");
            }

            return View(order);
        }
    }
}