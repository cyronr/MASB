using MediatR;
using AutoMapper;
using MABS.Application.DataAccess.Common;
using Microsoft.Extensions.Logging;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Common.AppProfile;
using MABS.Domain.Models.FacilityModels;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Application.ModelsExtensions.ProfileModelsExtensions;
using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Exceptions;
using MABS.Application.Features.FacilityFeatures.Common;

namespace MABS.Application.Features.FacilityFeatures.Commands.UpdateFacilityAddress
{
    public class UpdateFacilityAddressCommandHandler : IRequestHandler<UpdateFacilityAddressCommand, FacilityDto>
    {
        private readonly ILogger<UpdateFacilityAddressCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IFacilityRepository _facilityRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;

        public UpdateFacilityAddressCommandHandler(
            ILogger<UpdateFacilityAddressCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IFacilityRepository facilityRepository,
            ICurrentLoggedProfile currentLoggedProfile)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _facilityRepository = facilityRepository;
            _currentLoggedProfile = currentLoggedProfile;;
        }

        public async Task<FacilityDto> Handle(UpdateFacilityAddressCommand command, CancellationToken cancellationToken)
        {
            var callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile);

            _logger.LogDebug($"Fetching facility with id = {command.FacilityId}.");
            Facility facility = await new Facility().GetByUUIDAsync(_facilityRepository, command.FacilityId);

            _logger.LogDebug($"Fetching address with id = {command.Id}.");
            var address = facility.Addresses.Find(a => a.UUID == command.Id);
            if (address is null)
                throw new NotFoundException("Address not found.", $"FacilityId = {command.FacilityId}; AddressId = {command.Id}");

            _logger.LogDebug($"Fetching country with id = {command.CountryId}.");
            var country = await new Country().GetByIdAsync(_facilityRepository, command.CountryId);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    address.Name = command.Name;
                    address.StreetTypeId = (AddressStreetType.StreetType)command.StreetTypeId;
                    address.StreetName = command.StreetName;
                    address.HouseNumber = command.HouseNumber;
                    address.FlatNumber = command.FlatNumber;
                    address.City = command.City;
                    address.PostalCode = command.PostalCode;

                    _logger.LogInformation($"Checking if address ({address.ToString()}) already exists.");
                    await address.CheckAlreadyExistsAsync(_facilityRepository);

                    await _db.Save();

                    _facilityRepository.CreateEvent(new FacilityEvent
                    {
                        TypeId = FacilityEventType.Type.Created,
                        Facility = facility,
                        AddInfo = $"Updated address: {address.ToString()}",
                        CallerProfile = callerProfile.GetProfileEntity()
                    });

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
