namespace MABS.Application.UnitTests.Mocks;

public class MockProfileRepository
{
    public static Mock<IProfileRepository> GetProfileRepository()
    {
        var mockProfiles = PrepareListOfMockProfiles();

        return new Mock<IProfileRepository>().SetupRepository(mockProfiles);
    }

    private static List<Profile> PrepareListOfMockProfiles()
    {
        return new List<Profile>
        {
            new Profile
            {
                Id = 1,
                UUID = Guid.Parse(Consts.Active_Admin_Profile_UUID),
                StatusId = ProfileStatus.Status.Active,
                Status = new ProfileStatus
                {
                    Id = ProfileStatus.Status.Active,
                    Name = "Active"
                },
                TypeId = ProfileType.Type.Admin,
                Type = new ProfileType
                {
                    Id = ProfileType.Type.Admin,
                    Name = "Admin"
                },
                Email = "mock_admin@mabs.com",
                PhoneNumber = "123456789"
            },
            new Profile
            {
                Id = 2,
                UUID = Guid.NewGuid(),
                StatusId = ProfileStatus.Status.Active,
                Status = new ProfileStatus
                {
                    Id = ProfileStatus.Status.Active,
                    Name = "Active"
                },
                TypeId = ProfileType.Type.Doctor,
                Type = new ProfileType
                {
                    Id = ProfileType.Type.Doctor,
                    Name = "Doctor"
                },
                Email = "mock_doctor@mabs.com",
                PhoneNumber = "123456789"
            },
            new Profile
            {
                Id = 3,
                UUID = Guid.NewGuid(),
                StatusId = ProfileStatus.Status.Active,
                Status = new ProfileStatus
                {
                    Id = ProfileStatus.Status.Active,
                    Name = "Active"
                },
                TypeId = ProfileType.Type.Facility,
                Type = new ProfileType
                {
                    Id = ProfileType.Type.Facility,
                    Name = "Facility"
                },
                Email = "mock_facility@mabs.com",
                PhoneNumber = "123456789"
            },
            new Profile
            {
                Id = 4,
                UUID = Guid.NewGuid(),
                StatusId = ProfileStatus.Status.Active,
                Status = new ProfileStatus
                {
                    Id = ProfileStatus.Status.Active,
                    Name = "Active"
                },
                TypeId = ProfileType.Type.Patient,
                Type = new ProfileType
                {
                    Id = ProfileType.Type.Patient,
                    Name = "Patient"
                },
                Email = "mock_patient@mabs.com",
                PhoneNumber = "123456789"
            }
        };
    }
}

