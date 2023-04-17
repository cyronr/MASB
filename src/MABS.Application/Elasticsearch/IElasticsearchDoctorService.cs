using MABS.Application.Common.Pagination;
using MABS.Application.Elasticsearch.Models;

namespace MABS.Application.Elasticsearch
{
    public interface IElasticsearchDoctorService
    {
        Task<List<ElasticDoctor>> SearchAsync(string? searchText, int? specialtyId);
    }
}
