using Microsoft.AspNetCore.Localization;

namespace Hornbill.Emr.Api.Infrastructure.DependencyInjection;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAppLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = new[] { "en-US", "de-DE" };
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
        localizationOptions.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
        return app.UseRequestLocalization(localizationOptions);
    }
}
