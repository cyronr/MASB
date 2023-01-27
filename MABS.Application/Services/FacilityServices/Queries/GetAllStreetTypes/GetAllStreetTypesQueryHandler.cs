using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.FacilityServices.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Services.FacilityServices.Queries.GetAllStreetTypes
{
    public class GetAllStreetTypesQueryHandler : IRequestHandler<GetAllStreetTypesQuery, List<StreetTypeExtendedDto>>
    {

        private readonly ILogger<GetAllStreetTypesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IFacilityRepository _facilityRepository;

        public GetAllStreetTypesQueryHandler(
            ILogger<GetAllStreetTypesQueryHandler> logger, 
            IMapper mapper, 
            IFacilityRepository facilityRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _facilityRepository = facilityRepository;
        }


        public async Task<List<StreetTypeExtendedDto>> Handle(GetAllStreetTypesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Getting list of Countries.");

            var countries = await _facilityRepository.GetAllStreetTypesAsync();
            return countries.Select(c => _mapper.Map<StreetTypeExtendedDto>(c)).ToList();
        }
    }
}
