namespace Hornbill.Emr.Shared.Dtos.PatientManagement;

public record CreatePatientRequest(
    string FirstName,
    string MiddleName,
    string LastName,
    int GenderId,
    DateOnly DateOfBirth,
    int BloodGroupId);
