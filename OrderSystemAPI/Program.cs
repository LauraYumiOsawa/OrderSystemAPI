using Microsoft.EntityFrameworkCore;
using OrderSystemAPI.Data;

namespace OrderSystemAPI;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AppDbContext>(options =>
                // sqlserver?
            options.UseSqlServer("Server=localhost;Database=TalTal;Trusted_Connection=True;TrustServerCertificate=True;"));

        builder.Services.AddControllers();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        }

        app.MapControllers();

        app.Run();
    }
}