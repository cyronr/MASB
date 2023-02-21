using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.FacilityFeatures.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.FacilityFeatures.Queries.GetAllCountries
{
    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, List<CountryDto>>
    {

        private readonly ILogger<GetAllCountriesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IFacilityRepository _facilityRepository;

        public GetAllCountriesQueryHandler(
            ILogger<GetAllCountriesQueryHandler> logger,
            IMapper mapper,
            IFacilityRepository facilityRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _facilityRepository = facilityRepository;
        }


        public async Task<List<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Getting list of Countries.");

            var countries = await _facilityRepository.GetAllCountriesAsync();
            return countries.Select(c => _mapper.Map<CountryDto>(c)).ToList();
        }
    }
}
