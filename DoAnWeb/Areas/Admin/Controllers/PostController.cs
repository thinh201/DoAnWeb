using AspNetCoreHero.ToastNotification.Abstractions;
using DoAnWeb.Context;
using DoAnWeb.Models;
using DoAnWeb.Models.Authentication;
using DoAnWeb.Ultilities;
using DoAnWeb.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PagedList.Core;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthentication]
    public class PostController : Controller
    {
        private readonly MyDbContext _context;
        private readonly INotyfService _notyf;

        public PostController(MyDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;

            List<Blog> listBlog = new List<Blog>();
            listBlog = _context.Blogs.AsNoTracking().OrderByDescending(x => x.BlogId).ToList();

            PagedList<Blog> models = new PagedList<Blog>(listBlog.AsQueryable(), pageNumber, pageSize);
            ViewBag.currentPage = pageNumber;
            return View(models);
        }

        public IActionResult GoToTrash(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 5;

            List<Blog> listBlog = new List<Blog>();
            listBlog = _context.Blogs.AsNoTracking().OrderByDescending(x => x.BlogId).ToList();

            PagedList<Blog> models = new PagedList<Blog>(listBlog.AsQueryable(), pageNumber, pageSize);
            ViewBag.currentPage = pageNumber;
            return View(models);
        }

        [HttpGet]
        [Route("CreatePost")]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        [Route("CreatePost")]
        public async Task<IActionResult> CreatePost(Blog post, IFormFile? BlogImage)
        {
            if (post == null)
            {
                _notyf.Error("Thêm mới bài viết thất bại");
                return NotFound();
            }

            try
            {
                if (post.BlogTitle == null) TempData["BlogTitle"] = "Bạn phải nhập tên bài viết";
                if (post.BlogDesc == null) TempData["BlogDesc"] = "Bạn phải nhập tiêu đề bài viết";
                if (post.BlogDetail == null) TempData["BlogDetail"] = "Bạn phải nhập nội dung cho bài viết";
                if (post.BlogTitle == null ||
                    post.BlogDesc == null ||
                    post.BlogDetail == null)
                {
                    _notyf.Error("Thêm mới bài viết thất bại, kiểm tra lại thông tin nhập vào");
                    return View();
                }

                post.BlogSlug = Functions.AliasLink(post.BlogTitle);
                post.IsActive = true;
                post.CreatedDate = DateTime.Now;
                post.CreatedById = 1;
                post.BlogImage = BlogImage != null
                    ? UploadImage.UploadSingleImage(BlogImage)
                    : "/images/default-car.png";
                _context.Blogs.Add(post);
                await _context.SaveChangesAsync();
                _notyf.Success("Thêm mới bài viết thành công");
                return RedirectToAction("Index");
            }
            catch
            {
                _notyf.Error("Thêm mới bài viết thất bại");
                return NotFound();
            }
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var EditBlogById = _context.Blogs.Where(m => m.BlogId == id && m.IsActive == true).FirstOrDefault();
            if (EditBlogById == null)
            {
                return NotFound();
            }

            return View(EditBlogById);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Blog post, IFormFile? BlogImage)
        {
            var postById = _context.Blogs.Where(m => m.BlogId == id && m.IsActive == true).FirstOrDefault();
            if (postById == null)
            {
                _notyf.Error("Cập nhật bài viết thất bại");
                return NotFound();
            }

            try
            {
                if (post.BlogTitle == null) TempData["BlogTitle"] = "Bạn phải nhập tên bài viết";
                if (post.BlogDesc == null) TempData["BlogDesc"] = "Bạn phải nhập tiêu đề bài viết";
                if (post.BlogDetail == null) TempData["BlogDetail"] = "Bạn phải nhập nội dung cho bài viết";

                if (post.BlogTitle == null ||
                    post.BlogDesc == null ||
                    post.BlogDetail == null)
                {
                    _notyf.Error("Cập nhật bài viết thất bại, vui lòng kiểm tra lại thông tin nhập vào");
                    return View(postById);
                }

                postById.BlogSlug = Functions.AliasLink(post.BlogTitle);
                postById.BlogTitle = post.BlogTitle;
                postById.BlogDesc = post.BlogDesc;
                postById.BlogDetail = post.BlogDetail;
                postById.BlogImage = BlogImage != null
                    ? UploadImage.UploadSingleImage(BlogImage)
                    : postById.BlogImage;

                _context.Blogs.Update(postById);
                await _context.SaveChangesAsync();
                _notyf.Success("Cập nhật bài viết thành công");
                return RedirectToAction("Index");
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Delete(int IdToDelete)
        {
            if (IdToDelete == null)
            {
                return new JsonResult(new
                {
                    message = "Error",
                    status = 1
                });
            }

            try
            {
                var ItemById = await _context.Blogs.Where(m => m.IsActive == true && m.BlogId == IdToDelete)
                    .FirstOrDefaultAsync();
                if (ItemById == null)
                {
                    return new JsonResult(new
                    {
                        message = "Can not find User",
                        status = 1
                    });
                }
                else
                {
                    _context.Blogs.Remove(ItemById);
                    _context.SaveChanges();
                    return new JsonResult(new
                    {
                        message = "Success",
                        status = 0
                    });
                }
            }
            catch
            {
                return new JsonResult(new
                {
                    message = "Error from server",
                    status = 1
                });
            }
        }

        public async Task<IActionResult> UpdateActiveStatus(int IdToUpdate)
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
                var ItemById = await _context.Blogs.Where(m => m.BlogId == IdToUpdate).FirstOrDefaultAsync();
                if (ItemById == null)
                    return Json(new
                    {
                        status = 2,
                        message = "Cannot find Product"
                    });
                ItemById.IsActive = !ItemById.IsActive;
                _context.Blogs.Update(ItemById);
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