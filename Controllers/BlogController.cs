using FiorelloApp.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly FiorelloAppDbContext _context;

        public BlogController(FiorelloAppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Blogs.AsNoTracking().ToList());
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return BadRequest();
            var blog = _context.Blogs.AsNoTracking().FirstOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }
    }
}
