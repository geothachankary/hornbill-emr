using Hornbill.Emr.Api.Core.Entities;
using Hornbill.Emr.Api.Core.Enums;
using Hornbill.Emr.Api.Infrastructure.Persistence;
using Hornbill.Emr.Shared.Dtos.PatientManagement;

namespace Hornbill.Emr.Api.Features.PatientManagement.CreatePatient;

public class CreatePatientEndpoint : Endpoint<CreatePatientRequest>
{
    public AppDbContext DbContext { get; set; }

    public override void Configure()
    {
        Post("patients");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreatePatientRequest req, CancellationToken ct)
    {
        var patient = Patient.Create(
            req.FirstName,
            req.MiddleName,
            req.LastName,
            (Gender)req.GenderId,
            req.DateOfBirth,
            (BloodGroup)req.BloodGroupId);
        patient.SetPatientCode($"PAT{DateTime.Now.Ticks}");
        DbContext.Patients.Add(patient);
        await DbContext.SaveChangesAsync();
        await SendAsync(new { Data = "Patient Created", Id = patient.Id });
    }
}
