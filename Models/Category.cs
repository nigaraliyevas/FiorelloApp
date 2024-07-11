using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Models
{
    public class Category : BaseEntity
    {
        [Required,
            MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Desc { get; set; }
        public List<Product> Products { get; set; }
    }
}
