using MABS.Application.Elasticsearch;
using MABS.Application.Elasticsearch.Models;

namespace MABS.Application.UnitTests.Mocks.Elasticsearch;

public class MockElasticsearchDoctorService
{
    public static Mock<IElasticsearchDoctorService> GetDoctorRepository()
    {
        var mockDoctors = PrepareListOfMockDoctors();

        return new Mock<IElasticsearchDoctorService>().Setup(mockDoctors);
    }

    private static List<ElasticDoctor> PrepareListOfMockDoctors()
    {
        List<ElasticSpecialty> mockSpecalties = PrepareListOfMockSpecialties();
        List<ElasticFacility> mockFacilities = PrepareListOfMockFacilities();

        return new List<ElasticDoctor>
        {
            new ElasticDoctor
            {
                Id = 1,
                UUID = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                TitleShortName = "Dr.",
                TitleName = "Doctor",
                Specalities =
                {,
                    mockSpecalties.Single(t => t.Id == 1),
                    mockSpecalties.Single(t => t.Id == 2)
                },
                Facilities =
                {
                    mockFacilities.Single(f => f.Name == "Fac1")
                }
            },
            new ElasticDoctor
            {
                Id = 2,
                UUID = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                TitleShortName = "Dr.",
                TitleName = "Doctor",
                Specalities =
                {
                    mockSpecalties.Single(t => t.Id == 2),
                    mockSpecalties.Single(t => t.Id == 3)
                },
                Facilities =
                {
                    mockFacilities.Single(f => f.Name == "Fac1"),
                    mockFacilities.Single(f => f.Name == "Fac2")
                }
            },
            new ElasticDoctor
            {
                Id = 3,
                UUID = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Johnson",
                TitleShortName = "Dr.",
                TitleName = "Doctor",
                Specalities =
                {
                    mockSpecalties.Single(t => t.Id == 4)
                },
                Facilities =
                {
                    mockFacilities.Single(f => f.Name == "Fac1"),
                    mockFacilities.Single(f => f.Name == "Fac2")
                }
            },
            new ElasticDoctor
            {
                Id = 4,
                UUID = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Brown",
                TitleShortName = "Dr.",
                TitleName = "Doctor",
                Specalities =
                {
                    mockSpecalties.Single(t => t.Id == 4),
                    mockSpecalties.Single(t => t.Id == 5)
                },
                Facilities =
                {
                    mockFacilities.Single(f => f.Name == "Fac3"),
                    mockFacilities.Single(f => f.Name == "Fac4")
                }
            };
    }

    private static List<ElasticFacility> PrepareListOfMockFacilities()
    {
        return new List<ElasticFacility>
        {
            new ElasticFacility { Id = Guid.NewGuid(), ShortName = "Fac1", Name = "First Facility" },
            new ElasticFacility { Id = Guid.NewGuid(), ShortName = "Fac2", Name = "Second Facility" },
            new ElasticFacility { Id = Guid.NewGuid(), ShortName = "Fac3", Name = "Third Facility" },
            new ElasticFacility { Id = Guid.NewGuid(), ShortName = "Fac4", Name = "Fourth Facility" }
        };
    }

    private static List<ElasticSpecialty> PrepareListOfMockSpecialties()
    {
        return new List<ElasticSpecialty>
        {
            new ElasticSpecialty { Id = 1, Name= "Mock Speciality 1" },
            new ElasticSpecialty { Id = 2, Name= "Mock Speciality 2" },
            new ElasticSpecialty { Id = 3, Name= "Mock Speciality 3" },
            new ElasticSpecialty { Id = 4, Name= "Mock Speciality 4" },
            new ElasticSpecialty { Id = 5, Name= "Mock Speciality 5" }
        };
    }
}

