using Hornbill.Emr.Api.Core.Enums;

namespace Hornbill.Emr.Api;

public class RootEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/");
        AllowAnonymous();
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        Response = new RootResponse(BloodGroup.ABNegative, Gender.Male, Guid.NewGuid());
        return Task.CompletedTask;
    }
}

public record RootResponse(BloodGroup BloodGroup, Gender Gender, Guid Guid);
