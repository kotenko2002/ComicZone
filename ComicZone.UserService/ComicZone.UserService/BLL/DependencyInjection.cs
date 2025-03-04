using ComicZone.UserService.BLL.Messaging;
using ComicZone.UserService.BLL.Messaging.Publishers;
using ComicZone.UserService.BLL.Services.Auth;
using ComicZone.UserService.BLL.Services.FileStorage;
using Microsoft.Extensions.Options;
using Minio;

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
                .AddMinio(configuration)
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

        public static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Services.FileStorage.MinioConfig>(configuration.GetSection("Minio"));

            services.AddSingleton<IMinioClient>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<Services.FileStorage.MinioConfig>>().Value;

                return new MinioClient()
                    .WithEndpoint(config.Endpoint)
                    .WithCredentials(config.AccessKey, config.SecretKey)
                    .Build();
            });

            services.AddSingleton<IFileStorage, MinIOStorage>();

            return services;
        }
    }
}
