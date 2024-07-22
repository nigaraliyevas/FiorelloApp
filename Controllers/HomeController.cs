using FiorelloApp.DAL;
using FiorelloApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FiorelloAppDbContext _context;

        public HomeController(FiorelloAppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var demo = _context.Products.ToList();
            var homeVm = new HomeVM()
            {
                Sliders = _context.Sliders.
                AsNoTracking()
                .ToList(),
                SliderContent = _context.SliderContent
                .AsNoTracking()
                .Single(),
                Categories = _context.Categories
                .AsNoTracking()
                .ToList(),
                //Products = _context.Products
                //.Include(p => p.ProductImages)
                //.Include(p => p.Category)
                //.AsNoTracking()
                //.ToList(),
                Experts = _context.Experts
                .AsNoTracking()
                .ToList()
            };
            return View(homeVm);
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return BadRequest();
            var blog = _context.Products
                .AsNoTracking()
                .Include(p => p.ProductImages).FirstOrDefault(b => b.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }
        public IActionResult Search(string value)
        {
            var query = _context.Products.Include(p => p.ProductImages);
            var data = query.
                Where(p => p.Name.ToLower()
                .Contains
                (value.ToLower()))
                .Take(3)
                .ToList();
            return PartialView("_SearchProductPartialView", data);
        }
    }
}
