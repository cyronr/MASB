using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.FacilityServices.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Services.FacilityServices.Queries.GetAllFacilities
{
    public class GetAllFacilitiesQueryHandler : IRequestHandler<GetAllFacilitiesQuery, PagedList<FacilityDto>>
    {
        private readonly ILogger<GetAllFacilitiesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IFacilityRepository _facilityRepository;

        public GetAllFacilitiesQueryHandler(
            ILogger<GetAllFacilitiesQueryHandler> logger,
            IMapper mapper,
            IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PagedList<FacilityDto>> Handle(GetAllFacilitiesQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching facilities with paging parameters = {query.PagingParameters.ToString()}.");

            var facilities = await _facilityRepository.GetAllAsync();

            return PagedList<FacilityDto>.ToPagedList(
                facilities.Select(f => _mapper.Map<FacilityDto>(f)).ToList(),
                query.PagingParameters.PageNumber,
                query.PagingParameters.PageSize
            );
        }

    }
}
