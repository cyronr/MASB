using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;

namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockFacilityRepository
{
    public static Mock<IFacilityRepository> GetFacilityRepository()
    {
        var mockFacilities = PrepareListOfFacilities();
        var mockAddresses = PrepareListOfMockAddresses();
        var mockCountry = PrepareMockCountry();

        return new Mock<IFacilityRepository>().SetupRepository(mockFacilities, mockAddresses, mockCountry);
    }

    public static List<Facility> PrepareListOfFacilities()
    {
        var addresses = PrepareListOfMockAddresses();

        return new List<Facility>
        {
            new Facility
            {
                Id = 1,
                UUID = Guid.Parse(Consts.Active_Facility_UUID),
                StatusId = FacilityStatus.Status.Active,
                Status = new FacilityStatus
                {
                    Id = FacilityStatus.Status.Active,
                    Name = "Active"
                },
                ShortName = "Fac1",
                Name = "Facility 1",
                TaxIdentificationNumber = Consts.Active_Facility_TIN,
                Addresses =
                {
                    addresses.Single(t => t.Id == 1),
                    addresses.Single(t => t.Id == 2)
                },
                Doctors = MockDoctorRepository.PrepareListOfMockDoctors()
            },
            new Facility
            {
                Id = 2,
                UUID = Guid.NewGuid(),
                StatusId = FacilityStatus.Status.Active,
                Status = new FacilityStatus
                {
                    Id = FacilityStatus.Status.Active,
                    Name = "Active"
                },
                ShortName = "Fac2",
                Name = "Facility 2",
                TaxIdentificationNumber = "222222222222",
                Addresses =
                {
                    addresses.Single(t => t.Id == 3),
                    addresses.Single(t => t.Id == 4)
                }
            },
            new Facility
            {
                Id = 3,
                UUID = Guid.NewGuid(),
                StatusId = FacilityStatus.Status.Active,
                Status = new FacilityStatus
                {
                    Id = FacilityStatus.Status.Active,
                    Name = "Active"
                },
                ShortName = "Fac3",
                Name = "Facility 3",
                TaxIdentificationNumber = "33333333333",
                Addresses =
                {
                    addresses.Single(t => t.Id == 5)
                }
            }
        };
    }

    public static List<Address> PrepareListOfMockAddresses()
    {
        return new List<Address>
        {
            new Address
            {
                Id = 1,
                UUID = Guid.Parse(Consts.Active_Address_UUID),
                StatusId = AddressStatus.Status.Active,
                Status = new AddressStatus
                {
                    Id = AddressStatus.Status.Active,
                    Name = "Active"
                },
                Name = "Mock Address 1",
                StreetTypeId = AddressStreetType.StreetType.Street,
                StreetType = new AddressStreetType
                {
                    Id = AddressStreetType.StreetType.Street,
                    Name = "Street"
                },
                StreetName = "Mock Street Name",
                HouseNumber = 1,
                FlatNumber = 1
            },
            new Address
            {
                Id = 2,
                UUID = Guid.Parse(Consts.Active_AddressWithoutSchedules_UUID),
                StatusId = AddressStatus.Status.Active,
                Status = new AddressStatus
                {
                    Id = AddressStatus.Status.Active,
                    Name = "Active"
                },
                Name = "Mock Address 2",
                StreetTypeId = AddressStreetType.StreetType.Street,
                StreetType = new AddressStreetType
                {
                    Id = AddressStreetType.StreetType.Street,
                    Name = "Street"
                },
                StreetName = "Mock Street Name",
                HouseNumber = 2,
                FlatNumber = 2
            },
            new Address
            {
                Id = 3,
                UUID = Guid.NewGuid(),
                StatusId = AddressStatus.Status.Active,
                Status = new AddressStatus
                {
                    Id = AddressStatus.Status.Active,
                    Name = "Active"
                },
                Name = "Mock Address 3",
                StreetTypeId = AddressStreetType.StreetType.Street,
                StreetType = new AddressStreetType
                {
                    Id = AddressStreetType.StreetType.Street,
                    Name = "Street"
                },
                StreetName = "Mock Street Name",
                HouseNumber = 3,
                FlatNumber = 3
            },
            new Address
            {
                Id = 4,
                UUID = Guid.NewGuid(),
                StatusId = AddressStatus.Status.Active,
                Status = new AddressStatus
                {
                    Id = AddressStatus.Status.Active,
                    Name = "Active"
                },
                Name = "Mock Address 4",
                StreetTypeId = AddressStreetType.StreetType.Street,
                StreetType = new AddressStreetType
                {
                    Id = AddressStreetType.StreetType.Street,
                    Name = "Street"
                },
                StreetName = "Mock Street Name",
                HouseNumber = 4,
                FlatNumber = 4
            },
            new Address
            {
                Id = 5,
                UUID = Guid.NewGuid(),
                StatusId = AddressStatus.Status.Active,
                Status = new AddressStatus
                {
                    Id = AddressStatus.Status.Active,
                    Name = "Active"
                },
                Name = "Mock Address 5",
                StreetTypeId = AddressStreetType.StreetType.Street,
                StreetType = new AddressStreetType
                {
                    Id = AddressStreetType.StreetType.Street,
                    Name = "Street"
                },
                StreetName = "Mock Street Name",
                HouseNumber = 5,
                FlatNumber = 5
            }
        };
    }

    public static Country PrepareMockCountry()
    {
        return new Country
        {
            Id = "MC",
            Name = "Country"
        };
    }
}
