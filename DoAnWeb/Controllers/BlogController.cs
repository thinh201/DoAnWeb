using DoAnWeb.Context;
using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Controllers
{
    [Route("[controller]")]
    public class BLogController : Controller
    {
        private readonly MyDbContext _context;

        public BLogController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var blogs = _context.Blogs.Where(blog => blog.IsActive == true).ToList();
            return View(blogs);
        }

        [HttpGet]
        [Route("{id}/{slug}")]
        public IActionResult Detail(int id, string? slug)
        {
            var blog = _context.Blogs.Where(blog => blog.BlogId == id && blog.IsActive == true).SingleOrDefault();
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }
    }
}