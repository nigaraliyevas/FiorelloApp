using FiorelloApp.DAL;
using FiorelloApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloApp.Controllers
{
    public class BasketController : Controller
    {
        private readonly FiorelloAppDbContext _context;

        public BasketController(FiorelloAppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddBasket(int? id)
        {
            var existProduct = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
            if (existProduct is null) return NotFound();
            var basket = Request.Cookies["basket"];
            List<BasketVM> list;

            if (basket is null)
                list = new();
            else
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            var existProductBasket = list.FirstOrDefault(p => p.Id == existProduct.Id);

            if (existProductBasket is null)
                list.Add(new BasketVM() { Id = existProduct.Id, BasketCount = 1 });

            else
                existProductBasket.BasketCount++;
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(list));

            return RedirectToAction("Index", "home");
        }
        public IActionResult ShowBasket(int? id)
        {
            var basket = Request.Cookies["basket"];
            List<BasketVM> list;
            if (basket is null)
            {
                list = new();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (var basketItem in list)
                {
                    var existProduct = _context.Products
                    .Include(p => p.ProductImages).FirstOrDefault(p => p.Id == basketItem.Id);
                    basketItem.Id = existProduct.Id;
                    basketItem.Name = existProduct.Name;
                    basketItem.ImageURL = existProduct.ProductImages.FirstOrDefault(e => e.IsMain).ImageURL;
                    basketItem.Price = existProduct.Price;
                }
            }
            return View(list);
        }
        public IActionResult Delete(int id)
        {
            if (id == null) return BadRequest();
            var basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            var existProduct = basket.FirstOrDefault(p => p.Id == id);
            if (existProduct is null) return NotFound();
            else
            {
                basket.Remove(existProduct);
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            return RedirectToAction("ShowBasket", "basket");
        }
    }
}
