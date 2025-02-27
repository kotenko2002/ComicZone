using ComicZone.UserService.BLL.Services.Auth;

namespace ComicZone.UserService.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services
                .AddAuthService(configuration);
        }

        public static IServiceCollection AddAuthService(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services
                .AddScoped<IAuthService, AuthService>()
                .Configure<JwtConfig>(configuration.GetSection("Jwt"));
        }
    }
}
