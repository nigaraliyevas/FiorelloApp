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
            //var query = _context.Blogs.AsQueryable();
            //var count = query.Count();
            ViewBag.Count = _context.Blogs.AsQueryable().Count();
            //var datas = _context.Blogs.AsNoTracking().Take(3).ToList();
            return View();
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return BadRequest();
            var blog = _context.Blogs.AsNoTracking().FirstOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }
        public IActionResult LoadMore(int offset = 3)
        {
            var query = _context.Blogs.AsQueryable();
            var datas = query.AsNoTracking().Skip(offset).Take(3).ToList();
            return PartialView("_BlogPartialView", datas);
        }
        public IActionResult Search(string value)
        {
            var query = _context.Blogs.AsQueryable();
            var data = query.
                Where(b => b.Title.ToLower()
                .Contains
                (value.ToLower()))
                .Take(3)
                .ToList();
            return PartialView("_SearchPartialView", data);
        }
    }
}
