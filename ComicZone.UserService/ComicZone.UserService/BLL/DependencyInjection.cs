using ComicZone.UserService.BLL.Messaging;
using ComicZone.UserService.BLL.Messaging.Publishers;
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
                .AddMessaging(configuration)
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

        public static IServiceCollection AddMessaging(
           this IServiceCollection services,
           IConfiguration configuration
        )
        {
            return services
                .AddScoped<IUserEventPublisher, UserEventPublisher>()
                .Configure<RabbitMqConfig>(configuration.GetSection("RabbitMQ"));
        }
    }
}
