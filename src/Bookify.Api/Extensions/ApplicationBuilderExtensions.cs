using Bookify.Api.Middleware;
using Bookify.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
        
        using ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        context.Database.Migrate();
    }
    
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
    
    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();
        
        return app;
    }
}
