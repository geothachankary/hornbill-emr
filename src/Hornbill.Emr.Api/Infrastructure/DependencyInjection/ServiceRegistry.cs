using Hornbill.Emr.Api.Core.Entities;
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

        services.AddOpenIddictServices();
        services.AddAuthzServices();

        return services;
    }

    private static void AddAuthzServices(this IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

        services.AddAuthorization();
    }

    private static void AddOpenIddictServices(this IServiceCollection services)
    {
        services.AddOpenIddict()
                .AddCore(options => options.UseEntityFrameworkCore().UseDbContext<AppDbContext>())
                .AddServer(options =>
                {
                    options.AllowAuthorizationCodeFlow()
                           .RequireProofKeyForCodeExchange()
                           .AllowClientCredentialsFlow()
                           .AllowRefreshTokenFlow()
                           .AllowPasswordFlow()
                           .AcceptAnonymousClients()
                           .DisableScopeValidation();

                    options.SetAuthorizationEndpointUris("/connect/authorize")
                           .SetTokenEndpointUris("/connect/token")
                           .SetUserinfoEndpointUris("/connect/userinfo");

                    // Register the signing and encryption credentials.
                    options.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate()
                           .DisableAccessTokenEncryption();

                    options.UseAspNetCore()
                           .EnableTokenEndpointPassthrough()
                           .EnableAuthorizationEndpointPassthrough()
                           .EnableUserinfoEndpointPassthrough()
                           .DisableTransportSecurityRequirement();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });
    }
}
