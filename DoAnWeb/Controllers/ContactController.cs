using AspNetCoreHero.ToastNotification.Abstractions;
using DoAnWeb.Context;
using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Controllers
{
	public class ContactController : Controller
	{
		private readonly MyDbContext _context;
		private readonly INotyfService _otyfService;

		public ContactController(MyDbContext context, INotyfService otyfService)
		{
			_context = context;
			_otyfService = otyfService;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult SendMessage(Contact contact)
		{
			if(contact == null)
			{
				_otyfService.Error("Không thể gửi tin nhắn, kiểm tra lại thông tin");
				return View("Index");
			}
			try
			{
				if (contact.Email == null || contact.Name== null || contact.Subject == null || contact.Message == null)
				{
                    _otyfService.Error("Không thể gửi tin nhắn, kiểm tra lại thông tin");
                    return View("Index", contact);
                }
				_context.Contacts.Add(contact);
				_context.SaveChanges();
				_otyfService.Success("Đã gửi tin nhắn thành công");
				return RedirectToAction("Index");
			} catch
			{
                _otyfService.Error("Không thể gửi tin nhắn, kiểm tra lại thông tin");
                return View("Index", contact);
            }
		}
	}
}
