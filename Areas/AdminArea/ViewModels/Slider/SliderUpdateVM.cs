namespace FiorelloApp.Areas.AdminArea.ViewModels.Slider
{
    public class SliderUpdateVM
    {
        public IFormFile PhotoUpload { get; set; }
        public string Photo { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
