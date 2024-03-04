using Hornbill.Emr.Api.Core.Enums;

namespace Hornbill.Emr.Api.Core.Entities;

public class Patient
{
    private Patient(
        string firstName,
        string middleName,
        string lastName,
        Gender gender,
        DateOnly dateOfBirth,
        BloodGroup bloodGroup)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        BloodGroup = bloodGroup;
    }

    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string MiddleName { get; private set; }
    public string LastName { get; private set; }
    public string PatientCode { get; private set; }
    public Gender Gender { get; private set; }
    public BloodGroup BloodGroup { get; private set; }
    public DateOnly DateOfBirth { get; private set; }

    public void SetPatientCode(string patientCode)
    {
        PatientCode = patientCode;
    }

    public static Patient Create(string firstName,
        string middleName,
        string lastName,
        Gender gender,
        DateOnly dateOfBirth,
        BloodGroup bloodGroup)
    {
        return new Patient(firstName, middleName, lastName, gender, dateOfBirth, bloodGroup);
    }
}
