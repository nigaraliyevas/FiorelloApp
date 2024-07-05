using FiorelloApp.DAL;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly FiorelloAppDbContext _context;

        public ProductController(FiorelloAppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var product = _context.Products.FirstOrDefault();
            return View();
        }
    }
}
