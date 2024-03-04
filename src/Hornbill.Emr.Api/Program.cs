global using FastEndpoints;
using System.Text.Json;
using FastEndpoints.Swagger;
using Hornbill.Emr.Api.Infrastructure.DependencyInjection;
using Hornbill.Emr.Api.Infrastructure.Persistence;
using Hornbill.Emr.Api.Infrastructure.Serialization.JsonConverters;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder();

builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware"))
    .CreateLogger();

try
{
    builder.Host.UseSerilog(logger);
    builder.Services.AddFastEndpoints();

    builder.Services.AddHealthChecks();
    builder.Services.SwaggerDocument();
    builder.Services.AddCors();

    builder.Services
        .AddInfrastructureServices(options =>
        {
            options.DbContextOptions.ConnectionString = builder.Configuration.GetConnectionString("Default")!;
            options.DbContextOptions.EnableSensitiveLogging = true;
        });

    builder.Services.AddControllers();
    builder.Services.ConfigureOptions<LocalizationJsonOptions>();
    builder.Services.Configure<JsonOptions>(o =>
    {
        o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        o.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

    logger.Information("Starting up Hornbill.Emr Api...");

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // this needs to be corrected with actual urls which to be read from appsettings.json
    app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

    app.UseAppLocalization();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseFastEndpoints(c =>
    {
        c.Endpoints.RoutePrefix = "api";
        c.Errors.UseProblemDetails();
    });
    app.UseSwaggerGen();
    app.MapHealthChecks("/healthz");
    app.MapControllers();

    // this logic to be moved to a separate class to remove dependency of EF in Program.cs
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }

    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
