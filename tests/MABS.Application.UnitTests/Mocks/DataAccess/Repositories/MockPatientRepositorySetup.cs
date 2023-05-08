using MABS.Domain.Models.PatientModels;

namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockPatientRepositorySetup
{
    public static Mock<IPatientRepository> SetupRepository(
        this Mock<IPatientRepository> mockRepo,
        List<Patient> mockPatients)
    {

        mockRepo.Setup(r => r.GetByUUIDAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid uuid) =>
            {
                return mockPatients.FirstOrDefault(d => d.UUID == uuid && d.StatusId != PatientStatus.Status.Deleted);
            });

        mockRepo.Setup(r => r.Create(It.IsAny<Patient>()))
            .Callback((Patient Patient) => mockPatients.Add(Patient))
            .Verifiable();

        mockRepo.Setup(r => r.CreateEvent(It.IsAny<PatientEvent>())).Verifiable();

        return mockRepo;
    }
}
