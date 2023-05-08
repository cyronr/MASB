using MABS.Application.Elasticsearch;
using MABS.Application.Elasticsearch.Models;
using MABS.Domain.Models.DoctorModels;
using Moq;

namespace MABS.Application.UnitTests.Mocks.Elasticsearch;

public static class MockElasticsearchDoctorServiceSetup
{
    public static Mock<IElasticsearchDoctorService> Setup(
        this Mock<IElasticsearchDoctorService> mockService,
        List<ElasticDoctor> mockDoctors)
    {
        mockService.Setup(service => service.FindExact(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<int>>()))
            .ReturnsAsync((string firstName, string lastName, List<int> specialtiesIds) =>
            {
                return mockDoctors.SingleOrDefault(d =>
                    d.FirstName == firstName &&
                    d.LastName == lastName &&
                    d.Specalities.Any(s => specialtiesIds.Contains(s.Id))
                );
            });

        mockService.Setup(service => service.SearchAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync((string? searchText, int? specialtyId) =>
            {
                return mockDoctors.Where(d =>
                    d.FirstName.Contains(searchText) ||
                    d.LastName.Contains(searchText) ||
                    d.Specalities.Any(s => s.Name.Contains(searchText)) ||
                    d.TitleName.Contains(searchText) ||
                    d.TitleShortName.Contains(searchText) ||
                    d.Specalities.Any(s => s.Id == specialtyId)
                ).ToList();
            });

        return mockService;
    }
}
