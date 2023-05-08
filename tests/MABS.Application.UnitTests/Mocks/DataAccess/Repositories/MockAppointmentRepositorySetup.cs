using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ScheduleModels;

namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockAppointmentRepositorySetup
{
    public static Mock<IAppointmentRepository> SetupRepository(
        this Mock<IAppointmentRepository> mockRepo,
        List<Appointment> mockAppointments)
    {

        mockRepo.Setup(r => r.GetByUUIDAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid uuid) =>
            {
                return mockAppointments.FirstOrDefault(d => d.UUID == uuid);
            });

        mockRepo.Setup(r => r.GetByPatientAsync(It.IsAny<Patient>()))
            .ReturnsAsync((Patient patient) =>
            {
                return mockAppointments.Where(a => a.Patient.Id == patient.Id).ToList();
            });

        mockRepo.Setup(r => r.GetByDoctorAsync(It.IsAny<Doctor>()))
            .ReturnsAsync((Doctor doctor) =>
            {
                return mockAppointments.Where(a => a.Schedule.Doctor.Id == doctor.Id).ToList();
            });

        mockRepo.Setup(r => r.GetByAddressAsync(It.IsAny<Address>()))
            .ReturnsAsync((Address address) =>
            {
                return mockAppointments.Where(a => a.Schedule.Address.Id == address.Id).ToList();
            });

        mockRepo.Setup(r => r.GetByDoctorAndAddressAsync(It.IsAny<Doctor>(), It.IsAny<Address>()))
            .ReturnsAsync((Doctor doctor, Address address) =>
            {
                return mockAppointments.Where(a => 
                    a.Schedule.Doctor.Id == doctor.Id &&
                    a.Schedule.Address.Id == address.Id
                ).ToList();
            });

        mockRepo.Setup(r => r.GetByScheduleAsync(It.IsAny<Schedule>()))
            .ReturnsAsync((Schedule schedule) =>
            {
                return mockAppointments.Where(a => a.Schedule.Id == schedule.Id).ToList();
            });


        mockRepo.Setup(r => r.Create(It.IsAny<Appointment>()))
            .Callback((Appointment appointment) => mockAppointments.Add(appointment))
            .Verifiable();

        mockRepo.Setup(r => r.CreateEvent(It.IsAny<AppointmentEvent>())).Verifiable();

        return mockRepo;
    }
}
