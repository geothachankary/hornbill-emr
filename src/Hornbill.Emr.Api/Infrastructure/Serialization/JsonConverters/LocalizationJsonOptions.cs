using Hornbill.Emr.Api.Infrastructure.Localization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Hornbill.Emr.Api.Infrastructure.Serialization.JsonConverters;

public class LocalizationJsonOptions(
    IHttpContextAccessor httpContextAccessor,
    IServiceProvider serviceProvider) : IConfigureOptions<JsonOptions>
{
    public void Configure(JsonOptions options)
    {
        // Use the request services, if available, to be able to resolve
        // scoped services.
        // If there isn't a current HttpContext, just use the root service
        // provider.
        var services = httpContextAccessor.HttpContext?.RequestServices ?? serviceProvider;
        using var scope = services.CreateScope();
        var localizer = scope.ServiceProvider.GetRequiredService<IAppLocalizer>();
        options.SerializerOptions.Converters.Add(new LocalizationConverter(localizer));
    }
}
