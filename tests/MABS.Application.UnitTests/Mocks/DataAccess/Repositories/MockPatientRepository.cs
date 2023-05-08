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
        var patientProfile = MockProfileRepository.PrepareListOfMockProfiles().Single(p => p.TypeId == ProfileType.Type.Patient);

        return new List<Patient>
        {
            new Patient
            {
                Id = 1,
                UUID = Guid.NewGuid(),
                StatusId = PatientStatus.Status.Active,
                Status = new PatientStatus
                {
                    Id = PatientStatus.Status.Active,
                    Name = "Active"
                },
                Firstname = "Mock_Firstname_1",
                Lastname = "Mock_Lastname_1",   
                Profile = patientProfile,
                ProfileId = patientProfile.Id
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
                Lastname = "Mock_Lastname_2",
                Profile = patientProfile,
                ProfileId = patientProfile.Id
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
                Lastname = "Mock_Lastname_3",
                Profile = patientProfile,
                ProfileId = patientProfile.Id
            }
        };
    }

}

