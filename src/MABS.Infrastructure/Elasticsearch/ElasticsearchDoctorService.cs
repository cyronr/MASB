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


        public async Task<ElasticDoctor?> FindExact(string firstName, string lastName, List<int> specialtiesIds)
        {
            var result = await _elastic.SearchAsync<ElasticDoctor>(s => s
                .Index("doctors")
                .Query(q => q
                    .Bool(b => b
                        .Must(
                            m => m.Match(mq => mq.Field(f => f.FirstName).Query(firstName)),
                            m => m.Match(mq => mq.Field(f => f.LastName).Query(lastName)),
                            m => m.Terms(mq => mq.Field(f => f.Specalities.Select(s => s.Id)).Terms(specialtiesIds))
                        )
                    )
                )
            );

            return result.Documents.ToList().SingleOrDefault();
        }

        public async Task<List<ElasticDoctor>> SearchAsync(string? searchText, int? specialtyId)
        {
            var result = await _elastic.SearchAsync<ElasticDoctor>(s => s
                .Index("doctors")
                .Query(q => q
                    .Bool(b => b
                        .Must(m => m
                            .MultiMatch(mm => mm
                                .Fields(f => f
                                    .Field("specialities.name", 2)
                                    .Field("lastName", 1.5)
                                    .Field("firstName", 1.4)
                                    .Field("titleShortName", 0.8)
                                    .Field("titleName", 0.5)
                                )
                                .Query(searchText)
                                .Fuzziness(Fuzziness.Auto)
                                .Type(TextQueryType.MostFields)
                            )
                        )
                        .Filter(f => f
                            .Term(t => t
                                .Field("specalities.id")
                                .Value(specialtyId)
                            )
                        )
                    )
                )
                .Size(100)
            );

            return result.Documents.ToList();
        }
    }
}
