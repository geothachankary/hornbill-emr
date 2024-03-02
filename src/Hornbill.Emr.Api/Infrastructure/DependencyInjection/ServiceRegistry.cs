using Hornbill.Emr.Api.Core.Entities;
using Hornbill.Emr.Api.Infrastructure.Localization;
using Hornbill.Emr.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

namespace Hornbill.Emr.Api.Infrastructure.DependencyInjection;

public static class ServiceRegistry
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        Action<InfrastructureOptions> infraOptions)
    {
        var infraOptionsValue = new InfrastructureOptions();
        infraOptions.Invoke(infraOptionsValue);

        services.AddDbContext<AppDbContext>(
            options =>
            {
                options
                    .UseNpgsql(
                        infraOptionsValue.DbContextOptions.ConnectionString,
                        x => x.MigrationsHistoryTable("__ef_migrations_history"))
                    .UseSnakeCaseNamingConvention();

                options.EnableSensitiveDataLogging(infraOptionsValue.DbContextOptions.EnableSensitiveLogging);

                // Register the entity sets needed by OpenIddict.
                options.UseOpenIddict();
            });

        services.AddIdentityAuthServices();
        services.AddOpenIddictServices();
        services.AddLocalizationServices();

        return services;
    }

    private static void AddIdentityAuthServices(this IServiceCollection services)
    {
        // Add ASP.NET Identity
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<AppDbContext>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

        services.AddAuthorization();
    }

    private static void AddLocalizationServices(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Infrastructure/Localization");
        services.AddScoped<IAppLocalizer, AppLocalizer>();
    }

    private static void AddOpenIddictServices(this IServiceCollection services)
    {
        services.AddOpenIddict()
                .AddCore(options => options.UseEntityFrameworkCore().UseDbContext<AppDbContext>())
                .AddServer(options =>
                {
                    options.AllowPasswordFlow()
                           .AllowRefreshTokenFlow()
                           .AcceptAnonymousClients()
                           .DisableScopeValidation();

                    options.SetTokenEndpointUris("/connect/token");

                    // Register the signing and encryption credentials.
                    options.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate()
                           .DisableAccessTokenEncryption();

                    options.UseAspNetCore()
                           .EnableTokenEndpointPassthrough()
                           .DisableTransportSecurityRequirement();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });
    }
}
