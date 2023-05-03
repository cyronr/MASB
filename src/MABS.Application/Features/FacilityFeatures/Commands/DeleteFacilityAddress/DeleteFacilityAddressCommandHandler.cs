using AutoMapper;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.FacilityModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.FacilityFeatures.Commands.DeleteFacilityAddress
{
    public class DeleteFacilityAddressCommandHandler : IRequestHandler<DeleteFacilityAddressCommand, FacilityDto>
    {
        private readonly ILogger<DeleteFacilityAddressCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IFacilityRepository _facilityRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;
        private readonly IScheduleRepository _scheduleRepository;

        public DeleteFacilityAddressCommandHandler(
            ILogger<DeleteFacilityAddressCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IFacilityRepository facilityRepository,
            ICurrentLoggedProfile currentLoggedProfile,
            IScheduleRepository scheduleRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _facilityRepository = facilityRepository;
            _currentLoggedProfile = currentLoggedProfile;
            _scheduleRepository = scheduleRepository;
        }


        public async Task<FacilityDto> Handle(DeleteFacilityAddressCommand command, CancellationToken cancellationToken)
        {
            var callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile);

            _logger.LogDebug($"Fetching facility with id = {command.FacilityId}.");
            var facility = await new Facility().GetByUUIDAsync(_facilityRepository, command.FacilityId);

            _logger.LogDebug($"Fetching facility's address with id = {command.AddressId}.");
            var address = facility.Addresses.Find(a => a.UUID == command.AddressId);
            if (address is null)
                throw new NotFoundException("Adres o podanym identyfikatorze nie został istnieje");

            if (await AreSchdulesForAddress(address))
                throw new ConflictException("Istnieją aktywne harmonogramy dla wybranego adresu");

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    address.StatusId = AddressStatus.Status.Deleted;
                    await _db.Save();

                    _facilityRepository.CreateEvent(new FacilityEvent
                    {
                        TypeId = FacilityEventType.Type.Updated,
                        Facility = facility,
                        AddInfo = $"Removed address ({address.ToString()}).",
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

            facility.Addresses.Remove(address);
            return _mapper.Map<FacilityDto>(facility);
        }


        private async Task<bool> AreSchdulesForAddress(Address address)
        {
            _logger.LogDebug($"Fetching schedules for address = {address.Id}");
            var schedules = await _scheduleRepository.GetByAddressAsync(address);
            return schedules.Any();
        }
    }
}
