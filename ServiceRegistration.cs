using FiorelloApp.DAL;
using Microsoft.EntityFrameworkCore;

namespace FiorelloApp
{
    public static class ServiceRegistration
    {
        public static void Register(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews();
            services.AddDbContext<FiorelloAppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromSeconds(20);
            //});
            services.AddHttpContextAccessor();
        }
    }
}
