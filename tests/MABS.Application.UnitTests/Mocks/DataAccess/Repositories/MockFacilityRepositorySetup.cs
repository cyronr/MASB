using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ScheduleModels;
using Moq;

namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockFacilityRepositorySetup
{
    public static Mock<IFacilityRepository> SetupRepository(
        this Mock<IFacilityRepository> mockRepo,
        List<Facility> mockFacilities,
        List<Address> mockAddresses,
        Country mockCountry)
    {

        mockRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(() =>
            {
                return mockFacilities.Where(d =>
                    d.StatusId != FacilityStatus.Status.Deleted
                ).ToList();
            });

        mockRepo.Setup(r => r.GetByUUIDAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid uuid) =>
            {
                return mockFacilities.FirstOrDefault(d => d.UUID == uuid);
            });

        mockRepo.Setup(r => r.GetByTINAsync(It.IsAny<string>()))
           .ReturnsAsync((string tin) =>
           {
               return mockFacilities.FirstOrDefault(d => d.TaxIdentificationNumber == tin);
           });

        mockRepo.Setup(r => r.GetWithAllDoctorsByUUIDAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid uuid) =>
            {
                return mockFacilities.FirstOrDefault(d => d.UUID == uuid);
            });

        mockRepo.Setup(r => r.GetAddressByUUIDAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid uuid) =>
            {
                return mockAddresses.FirstOrDefault(d => d.UUID == uuid);
            });

        mockRepo.Setup(r => r.GetAddressesByDoctorAsync(It.IsAny<Doctor>()))
            .ReturnsAsync((Doctor doctor) =>
            {
                return mockAddresses.Where(a => 
                    a.StatusId == AddressStatus.Status.Active &&
                    a.Facility.StatusId != FacilityStatus.Status.Deleted &&
                    a.Facility.Doctors.Contains(doctor)
                ).ToList();
            });

        mockRepo.Setup(r => r.GetAddressByPropertiesAsync(
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<Country>())
        ).ReturnsAsync((string streetName, int houseNumber, int? flatNumber, string city, string postalCode, Country country) => {

            return mockAddresses.FirstOrDefault(a =>
                a.StatusId == AddressStatus.Status.Active &&
                a.StreetName == streetName &&
                a.HouseNumber == houseNumber &&
                a.FlatNumber == flatNumber &&
                a.City == city &&
                a.PostalCode == postalCode &&
                a.Country.Equals(country)
            );
        });

        mockRepo.Setup(r => r.GetCountryByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) =>
            {
                return mockCountry;
            });

        mockRepo.Setup(r => r.Create(It.IsAny<Facility>()))
            .Callback((Facility Facility) => mockFacilities.Add(Facility))
            .Verifiable();

        mockRepo.Setup(r => r.CreateEvent(It.IsAny<FacilityEvent>())).Verifiable();

        return mockRepo;
    }
}
