using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Areas.AdminArea.ViewModels.Category
{
    public class CategoryUpdateVM
    {
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Desc { get; set; }
    }
}
