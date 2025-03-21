using ComicZone.UserService.BLL;
using ComicZone.UserService.DAL;
using ComicZone.UserService.DAL.Common;
using ComicZone.UserService.PL;
using Microsoft.EntityFrameworkCore;

namespace ComicZone.UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddPresentationLayer(builder.Configuration)
                .AddBusinessLogicLayer(builder.Configuration)
                .AddDataAccessLayer(builder.Configuration);

            builder.Services
               .AddAuthScheme(builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            InitAutoMigration(app);

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        public static void InitAutoMigration(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                    Console.WriteLine("--> Migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> An error occurred while applying migrations: {ex.Message}");
                }
            }
        }
    }
}
