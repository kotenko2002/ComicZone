
using ComicZone.FileStorageService.BLL;
using ComicZone.FileStorageService.PL;
using ComicZone.FileStorageService.PL.Services;

namespace ComicZone.FileStorageService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddBusinessLogicLayer(builder.Configuration)
                .AddPresentationLayer(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.MapGrpcService<FileService>();

            app.Run();
        }
    }
}
