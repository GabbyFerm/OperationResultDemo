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
            builder.Services.AddControllers(); // Add support for [ApiController]s  
            builder.Services.AddEndpointsApiExplorer(); // Required for Swagger to discover endpoints  

            // Register Clean Architecture Dependency Injection  
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            // Add Swagger + JWT Authorization UI  
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddSwaggerWithJwt();

            var app = builder.Build();

            // Seed admin user on startup  
            DataSeeder.SeedAdminUser(app);

            // Configure Middleware Pipeline  
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Custom middleware for consistent error responses: It catches unhandled exeptions, unexpected crashes 
            // - UnauthorizedHandlingMiddleware: Converts 401/403 responses (from [Authorize]) into OperationResult JSON
            // - ExceptionHandlingMiddleware: Catches unhandled exceptions and logs + returns a safe OperationResult error
            app.UseMiddleware<UnauthorizedHandlingMiddleware>(); // Handles unauthenticated or unauthorized access
            app.UseMiddleware<ExceptionHandlingMiddleware>();    // Handles unexpected server-side exceptions

            app.UseHttpsRedirection();

            // Order matters: Authentication before Authorization  
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
