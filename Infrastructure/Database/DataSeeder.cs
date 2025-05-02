using Microsoft.AspNetCore.Builder;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Database
{
    public static class DataSeeder
    {
        public static void SeedAdminUser(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.Users.Any(u => u.Username == "admin"))
            {
                var admin = new User
                {
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin"
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
