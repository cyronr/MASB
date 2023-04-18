using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.DictionaryFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.DictionaryFeatures.Queries.GetAllCities
{
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, List<CityDto>>
    {
        private readonly ILogger<GetAllCitiesQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDictionaryRepository _dictionaryRepository;

        public GetAllCitiesQueryHandler(
            ILogger<GetAllCitiesQueryHandler> logger,
            IMapper mapper,
            IDictionaryRepository dictionaryRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _dictionaryRepository = dictionaryRepository;
        }

        public async Task<List<CityDto>> Handle(GetAllCitiesQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching all cities.");

            var cities = await _dictionaryRepository.GetAllCitiesAsync();
            return cities.Select(s => _mapper.Map<CityDto>(s)).ToList();
        }

    }
}
