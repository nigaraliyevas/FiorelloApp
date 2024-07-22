using FiorelloApp.Models;
using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.AdminArea.ViewModels.Product
{
    public class ProductUpdateVM
    {
        [MaxLength(150)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public IFormFile[]? UploadPhotos { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
    }
}
