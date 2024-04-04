using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Bookify.Api.Controllers.Bookings;
using Bookify.Api.Extensions;
using Bookify.Api.OpenApi;
using Bookify.Application;
using Bookify.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (ApiVersionDescription apiVersionDescription in app.DescribeApiVersions())
        {
           string url = $"/swagger/{apiVersionDescription.GroupName}/swagger.json"; 
           string name = apiVersionDescription.GroupName.ToUpperInvariant();
           options.SwaggerEndpoint(url, name);
        }
    });
    
    app.ApplyMigrations();
    
    // REMARK: Uncomment if you want to seed initial data for Integration test
#pragma warning disable S125
    // app.SeedData();
#pragma warning restore S125
}

app.UseHttpsRedirection();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

RouteGroupBuilder routeGroupBuilder = app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(apiVersionSet);

routeGroupBuilder.MapBookingEndpoints();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

public partial class Program;
