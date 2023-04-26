using AutoMapper;
using MediatR;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Models.FacilityModels;
using Microsoft.Extensions.Logging;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Domain.Exceptions;

namespace MABS.Application.Features.FacilityFeatures.Queries.GetFacilityByProfile
{
    public class GetFacilityByProfileQueryHandler : IRequestHandler<GetFacilityByProfileQuery, FacilityDto>
    {
        private readonly ILogger<GetFacilityByProfileQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IProfileRepository _profileRepository;

        public GetFacilityByProfileQueryHandler(
            ILogger<GetFacilityByProfileQueryHandler> logger,
            IMapper mapper,
            IFacilityRepository facilityRepository,
            IProfileRepository profileRepository)
        {
            _facilityRepository = facilityRepository;
            _logger = logger;
            _mapper = mapper;
            _profileRepository = profileRepository;
        }

        public async Task<FacilityDto> Handle(GetFacilityByProfileQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching facility id by profile (profileId = {query.Id}).");
            var faicilityId = await _profileRepository.GetFacilityIdByProfileIdAsync(query.Id);

            if (faicilityId is null)
                throw new NotFoundException("Nie znaleziono placówki dla podanego profilu.");

            _logger.LogDebug($"Fetching facility with id = {faicilityId}.");
            var facility = await new Facility().GetByUUIDAsync(_facilityRepository, (Guid)faicilityId);

            return _mapper.Map<FacilityDto>(facility);
        }

    }
}
