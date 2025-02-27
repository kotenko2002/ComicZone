using ComicZone.UserService.DAL.Common;
using ComicZone.UserService.DAL.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComicZone.UserService.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services
                .AddDbContext(configuration);
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
