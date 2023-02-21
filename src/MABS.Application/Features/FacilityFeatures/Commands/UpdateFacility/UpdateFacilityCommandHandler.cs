using MediatR;
using AutoMapper;
using MABS.Application.DataAccess.Common;
using Microsoft.Extensions.Logging;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Common.AppProfile;
using MABS.Domain.Models.FacilityModels;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Application.ModelsExtensions.ProfileModelsExtensions;
using MABS.Application.Features.FacilityFeatures.Common;

namespace MABS.Application.Features.FacilityFeatures.Commands.UpdateFacility
{
    public class UpdateFacilityCommandHandler : IRequestHandler<UpdateFacilityCommand, FacilityDto>
    {
        private readonly ILogger<UpdateFacilityCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IFacilityRepository _facilityRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;

        public UpdateFacilityCommandHandler(
            ILogger<UpdateFacilityCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IFacilityRepository facilityRepository,
            ICurrentLoggedProfile currentLoggedProfile)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _facilityRepository = facilityRepository;
            _currentLoggedProfile = currentLoggedProfile;
        }

        public async Task<FacilityDto> Handle(UpdateFacilityCommand command, CancellationToken cancellationToken)
        {
            var callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile);

            _logger.LogDebug($"Fetching facility with id = {command.Id}.");
            var facility = await new Facility().GetByUUIDAsync(_facilityRepository, command.Id);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    facility.ShortName = command.ShortName;
                    facility.Name = command.Name;

                    await _db.Save();

                    _facilityRepository.CreateEvent(new FacilityEvent
                    {
                        TypeId = FacilityEventType.Type.Created,
                        Facility = facility,
                        AddInfo = facility.ToString(),
                        CallerProfile = callerProfile.GetProfileEntity()
                    });
                    await _db.Save();

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }

            return _mapper.Map<FacilityDto>(facility);
        }
    }
}
