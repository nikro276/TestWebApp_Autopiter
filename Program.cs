using Microsoft.EntityFrameworkCore;
using TestWebApp_Autopiter.Models;

namespace TestWebApp_Autopiter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();
            var app = builder.Build();
            app.MapControllerRoute(
                name: "default", 
                pattern: "{controller=Utils}/{action=GetFilials}/{id?}");
            app.Run();
        }
    }
}