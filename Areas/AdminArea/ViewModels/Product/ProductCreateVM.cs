
using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.AdminArea.ViewModels.Product
{
    public class ProductCreateVM
    {
        [MaxLength(150)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public IFormFile[] UploadPhotos { get; set; }
    }
}
