using Hornbill.Emr.Api.Infrastructure.Persistence;
using Hornbill.Emr.Shared.Dtos.PatientManagement;
using Microsoft.EntityFrameworkCore;

namespace Hornbill.Emr.Api.Features.PatientManagement.GetPatient;

public class GetAllPatientsEndpoint : EndpointWithoutRequest<IEnumerable<PatientDetailsDto>>
{
    public AppDbContext DbContext { get; set; }

    public override void Configure()
    {
        Get("patients");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var patients = await DbContext.Patients
            .Select(p => new PatientDetailsDto(
                p.Id,
                p.FirstName,
                p.MiddleName,
                p.LastName,
                p.Gender.ToString(),
                p.DateOfBirth,
                p.BloodGroup.ToString(),
                p.PatientCode))
            .ToListAsync(cancellationToken: ct);
        await SendAsync(patients);
    }
}
