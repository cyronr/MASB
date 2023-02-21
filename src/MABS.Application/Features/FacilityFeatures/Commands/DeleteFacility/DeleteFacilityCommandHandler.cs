using AutoMapper;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Domain.Models.FacilityModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.FacilityFeatures.Commands.DeleteFacility
{
    public class DeleteFacilityCommandHandler : IRequestHandler<DeleteFacilityCommand, FacilityDto>
    {
        private readonly ILogger<DeleteFacilityCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IFacilityRepository _facilityRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;

        public DeleteFacilityCommandHandler(
            ILogger<DeleteFacilityCommandHandler> logger,
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

        public async Task<FacilityDto> Handle(DeleteFacilityCommand command, CancellationToken cancellationToken)
        {
            var callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile);

            _logger.LogDebug($"Fetching facility with id = {command.FacilityId}.");
            var facility = await new Facility().GetByUUIDAsync(_facilityRepository, command.FacilityId);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    facility.StatusId = FacilityStatus.Status.Deleted;
                    await _db.Save();

                    _facilityRepository.CreateEvent(new FacilityEvent
                    {
                        TypeId = FacilityEventType.Type.Deleted,
                        Facility = facility,
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
            };

            return _mapper.Map<FacilityDto>(facility);
        }
    }
}
