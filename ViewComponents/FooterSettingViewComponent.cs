using FiorelloApp.DAL;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloApp.ViewComponents
{
    public class FooterSettingViewComponent : ViewComponent
    {
        private readonly FiorelloAppDbContext _context;

        public FooterSettingViewComponent(FiorelloAppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = _context.Settings.ToDictionary(key => key.Key, value => value.Value);
            return View(await Task.FromResult(settings));
        }
    }
}
