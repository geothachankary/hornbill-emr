using Hornbill.Emr.Api.Core.Enums;
using Hornbill.Emr.Api.Infrastructure.Localization;

namespace Hornbill.Emr.Api;

public class RootEndpoint : EndpointWithoutRequest
{
    public IAppLocalizer Localizer { get; set; }

    public override void Configure()
    {
        Get("/");
        AllowAnonymous();
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        Response = new RootResponse(
            Localizer[nameof(BloodGroup.ABNegative)],
            Localizer[nameof(Gender.Male)],
            Guid.NewGuid());
        return Task.CompletedTask;
    }
}

public record RootResponse(string BloodGroup, string Gender, Guid Guid);
