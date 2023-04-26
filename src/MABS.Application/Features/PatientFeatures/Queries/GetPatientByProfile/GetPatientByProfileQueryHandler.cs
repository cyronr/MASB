using AutoMapper;
using MediatR;
using MABS.Application.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Domain.Exceptions;
using MABS.Application.Features.PatientFeatures.Common;
using MABS.Domain.Models.PatientModels;
using MABS.Application.ModelsExtensions.PatientModelsExtensions;

namespace MABS.Application.Features.PatientFeatures.Queries.GetPatientByProfile
{
    public class GetPatientByProfileQueryHandler : IRequestHandler<GetPatientByProfileQuery, PatientDto>
    {
        private readonly ILogger<GetPatientByProfileQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProfileRepository _profileRepository;
        private readonly IPatientRepository _patientRepository;

        public GetPatientByProfileQueryHandler(
            ILogger<GetPatientByProfileQueryHandler> logger,
            IMapper mapper,
            IProfileRepository profileRepository,
            IPatientRepository patientRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _profileRepository = profileRepository;
            _patientRepository = patientRepository;
        }

        public async Task<PatientDto> Handle(GetPatientByProfileQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching facility id by profile (profileId = {query.Id}).");
            var patientId = await _profileRepository.GetPatientIdByProfileIdAsync(query.Id);

            if (patientId is null)
                throw new NotFoundException("Nie znaleziono pacjenta dla podanego profilu.");

            _logger.LogDebug($"Fetching patient with id = {patientId}.");
            var facility = await new Patient().GetByUUIDAsync(_patientRepository, (Guid)patientId);

            return _mapper.Map<PatientDto>(facility);
        }

    }
}
