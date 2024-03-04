namespace Hornbill.Emr.Shared.Dtos.PatientManagement;

public record PatientDetailsDto(
    int Id,
    string FirstName,
    string MiddleName,
    string LastName,
    string Gender,
    DateOnly DateOfBirth,
    string BloodGroup,
    string PatientCode);
