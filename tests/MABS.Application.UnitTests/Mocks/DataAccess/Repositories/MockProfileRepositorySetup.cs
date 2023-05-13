using Moq;

namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockProfileRepositorySetup
{
    public static Mock<IProfileRepository> SetupRepository(
        this Mock<IProfileRepository> mockRepo,
        List<Profile> mockProfiles)
    {
        mockRepo.Setup(r => r.GetByUUIDAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid uuid) =>
            {
                return mockProfiles.FirstOrDefault(d => d.UUID == uuid && d.StatusId != ProfileStatus.Status.Deleted);
            });

        mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) =>
            {
                return mockProfiles.FirstOrDefault(d => d.Email == email && d.StatusId != ProfileStatus.Status.Deleted);
            });

        mockRepo.Setup(r => r.GetFacilityIdByProfileIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid uuid) =>
            {
                var facProfiles = mockProfiles.Where(p => p.Facility is not null).ToList();
                var facProfile = facProfiles.FirstOrDefault(d => d.Facility.UUID == uuid && d.StatusId != ProfileStatus.Status.Deleted);
                 if (facProfile is not null)
                 {
                     return facProfile.Facility.UUID;
                 }
                 else
                 {
                     return null;
                 }
            });

        mockRepo.Setup(r => r.GetPatientIdByProfileIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid uuid) =>
            {
                var patProfiles = mockProfiles.Where(p => p.Patient is not null).ToList();
                var patProfile = patProfiles.FirstOrDefault(d => d.Patient.UUID == uuid && d.StatusId != ProfileStatus.Status.Deleted);
                if (patProfile is not null)
                {
                    return patProfile.Patient.UUID;
                }
                else
                {
                    return null;
                }
            });

        mockRepo.Setup(r => r.Create(It.IsAny<Profile>()))
            .Callback((Profile Profile) => mockProfiles.Add(Profile))
            .Verifiable();

        mockRepo.Setup(r => r.CreateEvent(It.IsAny<ProfileEvent>())).Verifiable();

        return mockRepo;
    }
}
