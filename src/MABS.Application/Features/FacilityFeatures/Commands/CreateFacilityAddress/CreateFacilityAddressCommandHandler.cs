﻿using MediatR;
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
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.Common.Geolocation;

namespace MABS.Application.Features.FacilityFeatures.Commands.CreateFacilityAddress
{
    public class CreateFacilityAddressCommandHandler : IRequestHandler<CreateFacilityAddressCommand, FacilityDto>
    {
        private readonly ILogger<CreateFacilityAddressCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IFacilityRepository _facilityRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;
        private readonly IProfileRepository _profileRepository;
        private readonly IGeolocator _geolocator;

        public CreateFacilityAddressCommandHandler(
            ILogger<CreateFacilityAddressCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IFacilityRepository facilityRepository,
            ICurrentLoggedProfile currentLoggedProfile,
            IProfileRepository profileRepository,
            IGeolocator geolocator)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _facilityRepository = facilityRepository;
            _currentLoggedProfile = currentLoggedProfile;
            _profileRepository = profileRepository;
            _geolocator = geolocator;
        }

        public async Task<FacilityDto> Handle(CreateFacilityAddressCommand command, CancellationToken cancellationToken)
        {
            Profile profile;
            if (command.ProfileId is null)
                profile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();
            else
                profile = await new Profile().GetByUUIDAsync(_profileRepository, (Guid)command.ProfileId);

            Facility facility = await new Facility().GetByUUIDAsync(_facilityRepository, command.FacilityId);

            _logger.LogDebug($"Fetching country with id = {command.CountryId}.");
            var country = await new Country().GetByIdAsync(_facilityRepository, command.CountryId);

            if (!_db.IsActiveTransaction())
            {
                using (var tran = _db.BeginTransaction())
                {
                    try
                    {
                        await DoCreate(
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
                await DoCreate(
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

            return _mapper.Map<FacilityDto>(facility);
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

            GeoCoordinates coordinates = await _geolocator.EncodeAddress(address);
            address.Longitude = coordinates.Longitude;
            address.Latitude = coordinates.Latitude;

            facility.Addresses.Add(address);
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
