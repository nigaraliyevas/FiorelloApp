using FiorelloApp.DAL;
using FiorelloApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FiorelloApp.ViewComponents
{
    public class HeaderSettingViewComponent : ViewComponent
    {
        private readonly FiorelloAppDbContext _context;

        public HeaderSettingViewComponent(FiorelloAppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var basket = Request.Cookies["basket"];
            List<BasketVM> list;
            var settings = _context.Settings.ToDictionary(key => key.Key, value => value.Value);
            HeaderVM headerVM = default;
            if (basket != null)
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
                headerVM = new()
                {
                    Count = list.Count,
                    Settings = settings,
                    AllProducts = list
                };
            }

            return View(await Task.FromResult(headerVM));
        }
    }
}
