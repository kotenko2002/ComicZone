using ComicZone.FileStorageService.BLL.FileStorage;
using Microsoft.Extensions.Options;
using Minio;

namespace ComicZone.FileStorageService.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddMinio(configuration);
        }

        public static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileStorage.MinioConfig>(configuration.GetSection("Minio"));

            services.AddSingleton<IMinioClient>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<FileStorage.MinioConfig>>().Value;

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
