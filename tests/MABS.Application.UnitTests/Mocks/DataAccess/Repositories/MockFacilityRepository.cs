using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;

namespace MABS.Application.UnitTests.Mocks.DataAccess.Repositories;

public static class MockFacilityRepository
{
    public static Mock<IFacilityRepository> GetFacilityRepository()
    {
        var mockFacilities = new List<Facility>();
        var mockAddresses = new List<Address>();
        var mockCountry = new Country();

        return new Mock<IFacilityRepository>().SetupRepository(mockFacilities, mockAddresses, );
    }

    public static List<Address> PrepareListOfMockAddresses()
    {
        return new List<Address>
        {
            new Address
            {
                Id = 1,
                UUID = Guid.NewGuid(),
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
                StreetName = "Mock Street Name"

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
