using DoAnWeb.Context;
using DoAnWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Components
{
    [ViewComponent(Name = "RecentBlog")]
    public class RecentBlogComponent : ViewComponent
    {
        private MyDbContext _context;

        public RecentBlogComponent(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
        {
            var query = _context.Blogs.AsQueryable()
                .Where(b => b.IsActive == true)
                .OrderByDescending(b => b.BlogId)
                .Take(3);
            var blogs = await query.Select(b => new Blog
            {
                BlogId = b.BlogId,
                BlogTitle = b.BlogTitle,
                BlogImage = b.BlogImage,
                BlogSlug = b.BlogSlug,
                CreatedBy = b.CreatedBy,
                CreatedDate = b.CreatedDate,
            }).ToListAsync();
            return await Task.FromResult((IViewComponentResult)View(viewName, blogs));
        }
    }
}