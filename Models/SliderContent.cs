using System.ComponentModel.DataAnnotations;

namespace FiorelloApp.Models
{
    public class SliderContent : BaseEntity
    {
        [Required, MaxLength(35)]
        public string Title { get; set; }
        [Required, MaxLength(200)]
        public string Desc { get; set; }
        public string SignImageURL { get; set; }
    }
}
