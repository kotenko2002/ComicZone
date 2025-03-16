
using ComicZone.FileStorageService.BLL;
using ComicZone.FileStorageService.DAL;
using ComicZone.FileStorageService.PL;

namespace ComicZone.FileStorageService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddDataAccessLayer(builder.Configuration)
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

            app.Run();
        }
    }
}
