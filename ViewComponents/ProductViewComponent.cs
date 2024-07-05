using FiorelloApp.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly FiorelloAppDbContext _context;

        public ProductViewComponent(FiorelloAppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .ToList();
            return View(await Task.FromResult(products));
        }
    }
}
