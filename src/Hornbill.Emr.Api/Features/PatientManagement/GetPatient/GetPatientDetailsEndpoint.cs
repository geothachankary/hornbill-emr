using Hornbill.Emr.Api.Infrastructure.Persistence;
using Hornbill.Emr.Shared.Dtos.PatientManagement;
using Microsoft.EntityFrameworkCore;

namespace Hornbill.Emr.Api.Features.PatientManagement.GetPatient;

public class GetPatientDetailsEndpoint : EndpointWithoutRequest<PatientDetailsDto>
{
    public AppDbContext DbContext { get; set; }

    public override void Configure()
    {
        Get("patients/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var patientId = Route<int>("id");
        var patient = await DbContext.Patients.FirstOrDefaultAsync(p => p.Id == patientId, cancellationToken: ct);
        await SendAsync(new PatientDetailsDto(
            patient.Id,
            patient.FirstName,
            patient.MiddleName,
            patient.LastName,
            patient.Gender.ToString(),
            patient.DateOfBirth,
            patient.BloodGroup.ToString(),
            patient.PatientCode));
    }
}
