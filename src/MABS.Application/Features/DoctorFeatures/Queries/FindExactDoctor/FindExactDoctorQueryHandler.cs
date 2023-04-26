using MABS.Application.Elasticsearch;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.DoctorFeatures.Queries.FindExactDoctor
{
    public class SearchAllDoctorsQueryHandler : IRequestHandler<FindExactDoctorQuery, DoctorDto>
    {
        private readonly ILogger<SearchAllDoctorsQueryHandler> _logger;
        private readonly IElasticsearchDoctorService _elasticsearch;

        public SearchAllDoctorsQueryHandler(
            ILogger<SearchAllDoctorsQueryHandler> logger,
            IElasticsearchDoctorService elasticsearch)
        {
            _logger = logger;
            _elasticsearch = elasticsearch;
        }

        public async Task<DoctorDto?> Handle(FindExactDoctorQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching exact doctor by params ( FirstName = {query.firstName}, LastName = {query.lastName} )");

            var doctor = await _elasticsearch.FindExact(query.firstName, query.lastName, query.specialtiesIds);
            return doctor?.ConvertToDoctorDto() ?? null;
        }
    }
}
