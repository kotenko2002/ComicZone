namespace ComicZone.FileStorageService.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services;
        }
    }
}