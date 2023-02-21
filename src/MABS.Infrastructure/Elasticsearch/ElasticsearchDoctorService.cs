using MABS.Application.Common.Pagination;
using MABS.Application.Elasticsearch;
using MABS.Application.Elasticsearch.Models;
using MABS.Domain.Models.DoctorModels;
using Microsoft.Extensions.Logging;
using Nest;

namespace MABS.Infrastructure.Elasticsearch
{
    public class ElasticsearchDoctorService : IElasticsearchDoctorService
    {
        private readonly ILogger<ElasticsearchDoctorService> _logger;
        private readonly IElasticClient _elastic;

        public ElasticsearchDoctorService(ILogger<ElasticsearchDoctorService> logger, IElasticClient elastic)
        {
            _logger = logger;
            _elastic = elastic;
        }

        public async Task<List<ElasticDoctor>> SearchAsync(string searchText, int from, int size)
        {
            var result = await _elastic.SearchAsync<ElasticDoctor>(s => s
                .Index("doctors")
                .Query(q => q
                    .MultiMatch(c => c
                        .Fields(f => f
                            .Field(p => p.Specalities, 2)
                            .Field(p => p.LastName, 1.5)
                            .Field(p => p.FirstName, 1.4)
                            .Field(p => p.TitleName, 0.8)
                            .Field(p => p.TitleShortName, 0.5)
                         )
                        .Query(searchText)
                        .Fuzziness(Fuzziness.Auto)
                        .Type(TextQueryType.MostFields)
                    )
                )
                .From(from)
                .Size(size)
            );

            return result.Documents.ToList();
        }
    }
}
