using MABS.Application.Common.Pagination;
using MABS.Application.Elasticsearch;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.DoctorFeatures.Queries.SearchAllDoctorsByText
{
    public class SearchAllDoctorsByTextQueryHandler : IRequestHandler<SearchAllDoctorsByTextQuery, PagedList<DoctorDto>>
    {
        private readonly ILogger<SearchAllDoctorsByTextQueryHandler> _logger;
        private readonly IElasticsearchDoctorService _elasticsearch;

        public SearchAllDoctorsByTextQueryHandler(
            ILogger<SearchAllDoctorsByTextQueryHandler> logger,
            IElasticsearchDoctorService elasticsearch)
        {
            _logger = logger;
            _elasticsearch = elasticsearch;
        }

        public async Task<PagedList<DoctorDto>> Handle(SearchAllDoctorsByTextQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching doctors with paging parameters = {query.PagingParameters.ToString()}.");

            var doctors = await _elasticsearch.SearchAsync(query.SearchText, query.PagingParameters.PageNumber-1, query.PagingParameters.PageSize);

            return PagedList<DoctorDto>.ToPagedList(
                doctors.Select(d => d.ConvertToDoctorDto()).ToList(),
                query.PagingParameters.PageNumber,
                query.PagingParameters.PageSize
            );
        }

    }
}
