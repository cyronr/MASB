﻿using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.ScheduleModels;
using Moq;
using System.Net;
using System.Numerics;

namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockScheduleRepositorySetup
{
    public static Mock<IScheduleRepository> SetupRepository(
        this Mock<IScheduleRepository> mockRepo,
        List<Schedule> mockSchedules)
    {

        mockRepo.Setup(r => r.GetByUUIDAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid uuid) =>
            {
                return mockSchedules.FirstOrDefault(d => d.UUID == uuid && d.StatusId != ScheduleStatus.Status.Deleted);
            });

        mockRepo.Setup(r => r.GetByDoctorAsync(It.IsAny<Doctor>()))
            .ReturnsAsync((Doctor doctor) =>
            {
                var mockDoctorSchedules = mockSchedules.Where(s => s.Doctor is not null);
                return mockDoctorSchedules.Where(d => d.Doctor.Id == doctor.Id && d.StatusId != ScheduleStatus.Status.Deleted).ToList();
            });

        mockRepo.Setup(r => r.GetByAddressAsync(It.IsAny<Address>()))
            .ReturnsAsync((Address address) =>
            {
                var mockAddrressSchedules = mockSchedules.Where(s => s.Address is not null);
                return mockAddrressSchedules.Where(d => d.Address.Id == address.Id && d.StatusId != ScheduleStatus.Status.Deleted).ToList();
            });

        mockRepo.Setup(r => r.GetByDoctorAndAddressAsync(It.IsAny<Doctor>(), It.IsAny<Address>()))
            .ReturnsAsync((Doctor doctor, Address address) =>
            {
                var mockAddrressAndDoctorsSchedules = mockSchedules.Where(s => s.Address is not null && s.Doctor is not null);

                return mockAddrressAndDoctorsSchedules.Where(d =>
                    d.Doctor.Id == doctor.Id &&
                    d.Address.Id == address.Id && 
                    d.StatusId != ScheduleStatus.Status.Deleted
                ).ToList();
            });

        mockRepo.Setup(r => r.Create(It.IsAny<Schedule>()))
            .Callback((Schedule Schedule) => mockSchedules.Add(Schedule))
            .Verifiable();

        mockRepo.Setup(r => r.CreateEvent(It.IsAny<ScheduleEvent>())).Verifiable();

        return mockRepo;
    }
}
