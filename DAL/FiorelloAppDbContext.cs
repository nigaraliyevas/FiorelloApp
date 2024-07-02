using FiorelloApp.Models;
using Microsoft.EntityFrameworkCore;
namespace FiorelloApp.DAL
{
    public class FiorelloAppDbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderContent> SliderContent { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public FiorelloAppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
