using FiorelloApp.Areas.AdminArea.ViewModels.Category;
using FiorelloApp.DAL;
using FiorelloApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly FiorelloAppDbContext _context;

        public CategoryController(FiorelloAppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM categoryCreateVM)
        {
            if (!ModelState.IsValid) return View(categoryCreateVM);
            if (categoryCreateVM.Name == null) return NotFound();
            if (categoryCreateVM.Desc == null) return BadRequest();
            var existCategory = _context.Categories.ToList().Any(n => n.Name.ToLower() == categoryCreateVM.Name.ToLower());
            if (existCategory)
            {
                ModelState.AddModelError("Name", "Cannot be same Name");
                return View();
            };
            Category category = new()
            {
                Name = categoryCreateVM.Name,
                Desc = categoryCreateVM.Desc,
                CreatedDate = DateTime.Now,
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Category");
        }
        public IActionResult Update(int? id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            CategoryUpdateVM categoryUpdateVM = new CategoryUpdateVM()
            {
                Name = category.Name,
                Desc = category.Desc,
            };
            return View(categoryUpdateVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id, CategoryUpdateVM categoryUpdateVM)
        {
            if (!ModelState.IsValid) return View(categoryUpdateVM);
            if (id == null) return BadRequest();
            if (categoryUpdateVM == null) return NotFound();
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return BadRequest();
            var existCategory = _context.Categories.Any(n => n.Name.ToLower() == categoryUpdateVM.Name.ToLower() && n.Id != id); ;
            if (existCategory)
            {
                ModelState.AddModelError("Name", "This Category is exist");
                return View(categoryUpdateVM);
            };
            category.Name = categoryUpdateVM.Name;
            category.Desc = categoryUpdateVM.Desc;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return NotFound();
            var existCategory = await _context.Categories.FirstOrDefaultAsync(n => n.Id == id);
            _context.Categories.Remove(existCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Category");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();
            var existCategory = categories.FirstOrDefault(n => n.Id == id);
            if (id == null) return NotFound();
            if (existCategory == null) return NotFound();

            return View(existCategory);
        }
    }
}
