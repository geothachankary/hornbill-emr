using Hornbill.Emr.Api.Core.Enums;

namespace Hornbill.Emr.Api.Features.PatientManagement.CreatePatient;

public class CreatePatientEndpoint : Endpoint<CreatePatientRequest>
{
    public override void Configure()
    {
        Post("patients");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreatePatientRequest req, CancellationToken ct)
    {
        Logger.LogInformation("Patient {@patient}", req);
        await SendAsync(new { Data = "Patient Created" });
    }
}

public record CreatePatientRequest(int Id, string FirstName, string LastName, Gender Gender, DateOnly DateOfBirth, BloodGroup BloodGroup);
