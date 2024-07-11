using FiorelloApp.Areas.AdminArea.Extensions;
using FiorelloApp.Areas.AdminArea.ViewModels.Slider;
using FiorelloApp.DAL;
using FiorelloApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {
        private readonly FiorelloAppDbContext _context;

        public SliderController(FiorelloAppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders
                .AsNoTracking()
                .ToListAsync();
            return View(sliders);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM sliderCreateVM)
        {
            var file = sliderCreateVM.Photo;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "Can't be empty");
                return View(sliderCreateVM);
            }
            if (!file.CheckContentType("image"))
            {
                ModelState.AddModelError("Photo", "Only Image");
                return View(sliderCreateVM);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photo", "The Size is big");
                return View(sliderCreateVM);
            }
            Slider slider = new Slider() { ImageURL = await file.SaveFile(), CreatedDate = DateTime.Now };
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return BadRequest();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", slider.ImageURL);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return BadRequest();
            SliderUpdateVM sliderUpdateVM = new SliderUpdateVM() { Photo = slider.ImageURL };
            return View(sliderUpdateVM);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM sliderUpdateVM)
        {
            if (id == null) return BadRequest();
            var slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return BadRequest();
            var file = sliderUpdateVM.PhotoUpload;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "Can't be empty");
                sliderUpdateVM.Photo = slider.ImageURL;
                return View(sliderUpdateVM);
            }
            if (!file.CheckContentType("image"))
            {
                ModelState.AddModelError("Photo", "Only Image");
                return View(sliderUpdateVM);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photo", "The Size is big");
                return View(sliderUpdateVM);
            }
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", slider.ImageURL);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            slider.ImageURL = await file.SaveFile();
            _context.ChangeTracker.Entries();
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Entries();
            return RedirectToAction("Index");
        }
    }
}
