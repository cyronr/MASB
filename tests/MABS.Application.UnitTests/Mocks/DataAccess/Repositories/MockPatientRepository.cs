using MABS.Domain.Models.PatientModels;

namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockPatientRepository
{
    public static Mock<IPatientRepository> GetPatientRepository()
    {
        var mockPatients = PrepareListOfMockPatients();

        return new Mock<IPatientRepository>().SetupRepository(mockPatients);
    }

    public static List<Patient> PrepareListOfMockPatients()
    {
        return new List<Patient>
        {
            new Patient
            {
                Id = 1,
                UUID = Guid.Parse(Consts.Active_Patient_UUID),
                StatusId = PatientStatus.Status.Active,
                Status = new PatientStatus
                {
                    Id = PatientStatus.Status.Active,
                    Name = "Active"
                },
                Firstname = "Mock_Firstname_1",
                Lastname = "Mock_Lastname_1",
                Profile = new Profile
                {
                    Email = "mock@mock.mabs"
                }
            },
            new Patient
            {
                Id = 2,
                UUID = Guid.NewGuid(),
                StatusId = PatientStatus.Status.Deleted,
                Status = new PatientStatus
                {
                    Id = PatientStatus.Status.Deleted,
                    Name = "Active"
                },
                Firstname = "Mock_Firstname_2",
                Lastname = "Mock_Lastname_2"
            },
            new Patient
            {
                Id = 3,
                UUID = Guid.NewGuid(),
                StatusId = PatientStatus.Status.Active,
                Status = new PatientStatus
                {
                    Id = PatientStatus.Status.Active,
                    Name = "Active"
                },
                Firstname = "Mock_Firstname_3",
                Lastname = "Mock_Lastname_3"
            }
        };
    }

}

