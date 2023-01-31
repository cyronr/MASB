namespace MABS.Application.UnitTests.Mocks;

public static class MockProfileRepositorySetup
{
    public static Mock<IProfileRepository> SetupRepository(
        this Mock<IProfileRepository> mockRepo,
        List<Profile> mockProfiles
        )
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

        mockRepo.Setup(r => r.Create(It.IsAny<Profile>()))
            .Callback((Profile Profile) => mockProfiles.Add(Profile))
            .Verifiable();

        mockRepo.Setup(r => r.CreateEvent(It.IsAny<ProfileEvent>())).Verifiable();

        return mockRepo;
    }
}
