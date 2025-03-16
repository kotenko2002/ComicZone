using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace ComicZone.FileStorageService.PL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationLayer(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddControllers();
            services.AddGrpc();

            return services
               .AddHttpContextAccessor()
               .AddEndpointsApiExplorer()
               .AddSwagger()
               .AddProblemDetails();
        }

        private static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme (\"Bearer {token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }
    }
}
