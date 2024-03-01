using Hornbill.Emr.Api.Core.Enums;

namespace Hornbill.Emr.Api.Core.Entities;

public class Patient
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public BloodGroup BloodGroup { get; set; }
    public string PatientCode { get; set; }
}
