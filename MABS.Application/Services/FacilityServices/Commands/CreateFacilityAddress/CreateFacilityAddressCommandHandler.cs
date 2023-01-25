using MediatR;
using MABS.Application.Services.FacilityServices.Common;
using MABS.Services.FacilityServices.Commands.CreateFacility;
using AutoMapper;
using MABS.Application.DataAccess.Common;
using Microsoft.Extensions.Logging;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Common.AppProfile;
using MABS.Domain.Models.FacilityModels;
using Profile = MABS.Domain.Models.ProfileModels.Profile;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Application.ModelsExtensions.ProfileModelsExtensions;
using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.Services.FacilityServices.Commands.CreateFacilityAddress
{
    public class CreateFacilityAddressCommandHandler : IRequestHandler<CreateFacilityAddressCommand, AddressDto>
    {
        private readonly ILogger<CreateFacilityAddressCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IFacilityRepository _facilityRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;
        private readonly IProfileRepository _profileRepository;

        public CreateFacilityAddressCommandHandler(
            ILogger<CreateFacilityAddressCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IFacilityRepository facilityRepository,
            ICurrentLoggedProfile currentLoggedProfile,
            IProfileRepository profileRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _facilityRepository = facilityRepository;
            _currentLoggedProfile = currentLoggedProfile;
            _profileRepository = profileRepository;
        }

        public async Task<AddressDto> Handle(CreateFacilityAddressCommand command, CancellationToken cancellationToken)
        {
            Profile profile;
            if (command.ProfileId is null)
                profile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();
            else
                profile = await new Profile().GetByUUIDAsync(_profileRepository, (Guid)command.ProfileId);

            Facility facility = await new Facility().GetByUUIDAsync(_facilityRepository, command.FacilityId);

            _logger.LogDebug($"Fetching country with id = {command.CountryId}.");
            var country = await new Country().GetByIdAsync(_facilityRepository, command.CountryId);

            Address address;
            if (!_db.IsActiveTransaction())
            {
                using (var tran = _db.BeginTransaction())
                {
                    try
                    {
                        address = await DoCreate(
                            command.Name, 
                            (AddressStreetType.StreetType)command.StreetTypeId, 
                            command.StreetName,
                            command.HouseNumber,
                            command.FlatNumber,
                            command.City,
                            command.PostalCode,
                            country,
                            facility,
                            profile
                        );

                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
            else
                address = await DoCreate(
                    command.Name,
                    (AddressStreetType.StreetType)command.StreetTypeId,
                    command.StreetName,
                    command.HouseNumber,
                    command.FlatNumber,
                    command.City,
                    command.PostalCode,
                    country,
                    facility,
                    profile
                );

            return _mapper.Map<AddressDto>(address);
        }

        private async Task<Address> DoCreate(
            string name,
            AddressStreetType.StreetType streetType,
            string streetName,
            int houseNumber,
            int? flatNumber,
            string city,
            string postalCode,
            Country country,
            Facility facility,
            Profile profile)
        {
            Address address = new Address
            {
                UUID = Guid.NewGuid(),
                StatusId = AddressStatus.Status.Active,
                Name = name,
                StreetTypeId = streetType,
                StreetName = streetName,
                HouseNumber = houseNumber,
                FlatNumber = flatNumber,
                City = city,
                PostalCode = postalCode,
                Country = country,
                Facility = facility
            };

            _logger.LogInformation($"Checking if address ({address.ToString()}) already exists.");
            await address.CheckAlreadyExistsAsync(_facilityRepository);

            await _db.Save();

            _facilityRepository.CreateEvent(new FacilityEvent
            {
                TypeId = FacilityEventType.Type.Created,
                Facility = facility,
                AddInfo = $"Added new address: {address.ToString()}",
                CallerProfile = profile
            });
            await _db.Save();

            return address;
        }
    }
}
