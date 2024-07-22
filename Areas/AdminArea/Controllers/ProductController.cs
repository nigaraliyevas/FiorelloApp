using FiorelloApp.Areas.AdminArea.Extensions;
using FiorelloApp.Areas.AdminArea.Helpers;
using FiorelloApp.Areas.AdminArea.ViewModels.Product;
using FiorelloApp.DAL;
using FiorelloApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ProductController : Controller
    {
        private readonly FiorelloAppDbContext _context;

        public ProductController(FiorelloAppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM productCreateVM)
        {
            ViewBag.Categories = _context.Categories.ToList();
            if (!ModelState.IsValid) return View(productCreateVM);
            var files = productCreateVM.UploadPhotos;
            List<ProductImage> list = new List<ProductImage>();
            Product newProduct = new Product();

            if (files.Length == 0)
            {
                ModelState.AddModelError("UploadPhotos", "Can't be empty");
                return View(productCreateVM);
            }
            foreach (var file in files)
            {
                if (!file.CheckContentType("image"))
                {
                    ModelState.AddModelError("UploadPhotos", "Only Image");
                    return View(productCreateVM);
                }
                if (file.CheckSize(500))
                {
                    ModelState.AddModelError("UploadPhotos", "The Size is big");
                    return View(productCreateVM);
                }
                ProductImage productImage = new();
                productImage.ProductId = newProduct.Id;
                productImage.ImageURL = await file.SaveFile();
                if (files[0] == file)
                    productImage.IsMain = true;
                list.Add(productImage);
            }

            if (_context.Products.Any(p => p.Name.ToLower() == productCreateVM.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "The product name can't be same");
                return View(productCreateVM);
            }
            newProduct.ProductImages = list;
            newProduct.Name = productCreateVM.Name;
            newProduct.Price = productCreateVM.Price;
            newProduct.CategoryId = productCreateVM.CategoryId;
            newProduct.Count = productCreateVM.Count;
            newProduct.CreatedDate = DateTime.Now;

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            var products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (id == null) return NotFound();
            if (products == null) return NotFound();

            return View(products);
        }
        public async Task<IActionResult> SetMainPhoto(int? id)
        {
            var image = await _context.ProductImages
                .FirstOrDefaultAsync(p => p.Id == id);

            if (id == null) return NotFound();
            if (image == null) return NotFound();
            image.IsMain = true;
            var mainImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsMain && i.ProductId == image.ProductId);
            mainImage.IsMain = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = image.ProductId });
        }
        //Get
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            if (!ModelState.IsValid) return View();
            if (id == null) return NotFound();
            var product = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            ProductUpdateVM productUpdateVM = new()
            {
                Price = product.Price,
                CategoryId = product.CategoryId,
                Count = product.Count,
                Name = product.Name,
                ProductImages = product.ProductImages,
            };
            return View(productUpdateVM);
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null) return NotFound();
            var image = _context.ProductImages.FirstOrDefault(i => i.Id == id);
            if (image == null) return NotFound();
            if (image.IsMain) return BadRequest();
            Helper.DeleteImageFromFolder(image.ImageURL);
            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction("Update", new { id = image.ProductId });
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> Update(int id, ProductUpdateVM productUpdateVM)
        {
            if (id == null) return NotFound();
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            productUpdateVM.ProductImages = product.ProductImages;
            if (!ModelState.IsValid) return View(productUpdateVM);
            var files = productUpdateVM.UploadPhotos;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (!file.CheckContentType("image"))
                    {
                        ModelState.AddModelError("UploadPhotos", "Only Image");
                        return View(productUpdateVM);
                    }
                    if (file.CheckSize(500))
                    {
                        ModelState.AddModelError("UploadPhotos", "The Size is big");
                        return View(productUpdateVM);
                    }
                    ProductImage productImage = new();
                    productImage.ProductId = product.Id;
                    productImage.ImageURL = await file.SaveFile();
                    productImage.IsMain = false;
                    product.ProductImages.Add(productImage);
                }
            }
            product.Name = productUpdateVM.Name;
            product.Price = productUpdateVM.Price;
            product.CategoryId = productUpdateVM.CategoryId;
            product.Count = productUpdateVM.Count;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
