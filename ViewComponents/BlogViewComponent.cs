using FiorelloApp.DAL;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloApp.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly FiorelloAppDbContext _context;

        public BlogViewComponent(FiorelloAppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int take = 3)
        {
            var blogs = _context.Blogs.Take(take).ToList();
            return View(await Task.FromResult(blogs));
        }
    }
}
