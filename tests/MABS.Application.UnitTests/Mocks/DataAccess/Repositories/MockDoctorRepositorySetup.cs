namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockDoctorRepositorySetup
{
    public static Mock<IDoctorRepository> SetupRepository(
        this Mock<IDoctorRepository> mockRepo,
        List<Doctor> mockDoctors,
        List<Title> mockTitles,
        List<Specialty> mockSpecialties
        )
    {
        mockRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(() =>
            {
                return mockDoctors.Where(d =>
                    d.StatusId != DoctorStatus.Status.Deleted
                ).ToList();
            });

        mockRepo.Setup(r => r.GetByUUIDAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid uuid) =>
            {
                return mockDoctors.FirstOrDefault(d => d.UUID == uuid && d.StatusId != DoctorStatus.Status.Deleted);
            });

        mockRepo.Setup(r => r.GetBySpecaltiesAsync(It.IsAny<List<int>>()))
            .ReturnsAsync((List<int> ids) =>
            {
                return mockDoctors.Where(d =>
                    d.StatusId != DoctorStatus.Status.Deleted
                    && d.Specialties.Any(s => ids.Contains(s.Id))
                ).ToList();
            });

        mockRepo.Setup(r => r.Create(It.IsAny<Doctor>()))
            .Callback((Doctor doctor) => mockDoctors.Add(doctor))
            .Verifiable();

        mockRepo.Setup(r => r.CreateEvent(It.IsAny<DoctorEvent>())).Verifiable();

        mockRepo.Setup(r => r.GetTitleByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) =>
            {
                return mockTitles.FirstOrDefault(t => t.Id == id);
            });

        mockRepo.Setup(r => r.GetAllTitlesAsync()).ReturnsAsync(mockTitles);

        mockRepo.Setup(r => r.GetSpecialtyByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) =>
            {
                return mockSpecialties.FirstOrDefault(t => t.Id == id);
            });

        mockRepo.Setup(r => r.GetAllSpecialtiesAsync()).ReturnsAsync(mockSpecialties);

        mockRepo.Setup(r => r.SetElasticsearchSyncNeeded(It.IsAny<long>())).Verifiable();

        return mockRepo;
    }
}
