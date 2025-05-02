using Api.Middleware;
using Application;
using Infrastructure;
using Infrastructure.Database;
using Infrastructure.Extensions;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.  
            builder.Services.AddControllers();   
            builder.Services.AddEndpointsApiExplorer(); // Enables Swagger to discover endpoints 

            // Register Clean Architecture Dependency Injection  
            builder.Services.AddApplication(); // Application layer services
            builder.Services.AddInfrastructure(builder.Configuration); // Infrastructure services

            // Add Swagger with JWT Authorization support  
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddSwaggerWithJwt();

            var app = builder.Build();

            // Seed admin user on startup (only if it doesn't exist)  
            DataSeeder.SeedAdminUser(app);

            // Configure Middleware Pipeline  
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Custom middleware for consistent error responses: 
            app.UseMiddleware<UnauthorizedHandlingMiddleware>(); // Handles unauthenticated/unauthorized access
            app.UseMiddleware<ExceptionHandlingMiddleware>();    // Handles unexpected runtime exceptions

            app.UseHttpsRedirection();

            // Authentication must come before Authorization    
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
