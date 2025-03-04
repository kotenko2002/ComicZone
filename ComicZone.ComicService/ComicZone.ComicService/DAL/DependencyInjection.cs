using ComicZone.ComicService.DAL.Common;
using ComicZone.ComicService.DAL.Uow;
using Microsoft.EntityFrameworkCore;

namespace ComicZone.ComicService.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services
                .AddDbContext(configuration)
                .AddScoped<IUnitOfWork, UnitOfWork>(); ;
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
