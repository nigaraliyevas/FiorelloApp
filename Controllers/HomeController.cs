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
                Products = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .AsNoTracking()
                .ToList(),
                Experts = _context.Experts
                .AsNoTracking()
                .ToList(),
                Blogs = _context.Blogs
                .Take(3)
                .AsNoTracking()
                .ToList(),
            };
            return View(homeVm);
        }
    }
}
