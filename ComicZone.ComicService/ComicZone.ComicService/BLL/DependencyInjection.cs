using ComicZone.ComicService.BLL.Messaging;
using ComicZone.ComicService.BLL.Messaging.Consumers;

namespace ComicZone.ComicService.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddMessaging(configuration);
        }

        public static IServiceCollection AddMessaging(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .Configure<RabbitMqConfig>(configuration.GetSection("RabbitMQ"))
                .AddHostedService<UserCreatedConsumer>();
        }
    }
}
