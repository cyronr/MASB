using MediatR;
using MABS.Application.Services.FacilityServices.Common;
using AutoMapper;
using MABS.Application.DataAccess.Common;
using Microsoft.Extensions.Logging;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Common.AppProfile;
using MABS.Domain.Models.FacilityModels;
using Profile = MABS.Domain.Models.ProfileModels.Profile;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Application.ModelsExtensions.ProfileModelsExtensions;

namespace MABS.Application.Services.FacilityServices.Commands.CreateFacility
{
    public class CreateFacilityCommandHandler : IRequestHandler<CreateFacilityCommand, FacilityDto>
    {
        private readonly ILogger<CreateFacilityCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IDbOperation _db;
        private readonly IFacilityRepository _facilityRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;
        private readonly IProfileRepository _profileRepository;

        public CreateFacilityCommandHandler(
            ILogger<CreateFacilityCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IFacilityRepository facilityRepository,
            ICurrentLoggedProfile currentLoggedProfile,
            IProfileRepository profileRepository,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _facilityRepository = facilityRepository;
            _currentLoggedProfile = currentLoggedProfile;
            _profileRepository = profileRepository;
            _mediator = mediator;
        }

        public async Task<FacilityDto> Handle(CreateFacilityCommand command, CancellationToken cancellationToken)
        {
            Profile profile;
            if (command.ProfileId is null)
                profile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();
            else
                profile = await new Profile().GetByUUIDAsync(_profileRepository, (Guid)command.ProfileId);

            Guid facilityUUID;
            if (!_db.IsActiveTransaction())
            {
                using (var tran = _db.BeginTransaction())
                {
                    try
                    {
                        var facility = await DoCreate(command.ShortName, command.Name, command.TaxIdentificationNumber, profile);
                        facilityUUID = facility.UUID;

                        command.Address.FacilityId = facility.UUID;
                        await _mediator.Send(command.Address);

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
            {
                var facility = await DoCreate(command.ShortName, command.Name, command.TaxIdentificationNumber, profile);
                facilityUUID = facility.UUID;

                command.Address.FacilityId = facility.UUID;
                command.Address.ProfileId = profile.UUID;
                await _mediator.Send(command.Address);
            }
                
            return _mapper.Map<FacilityDto>(
                await new Facility().GetByUUIDAsync(_facilityRepository, facilityUUID)
            );
        }

        private async Task<Facility> DoCreate(
            string shortName,
            string name,
            string taxIdentificationNumber,
            Profile profile)
        {
            Facility facility = new Facility
            {
                UUID = Guid.NewGuid(),
                StatusId = FacilityStatus.Status.Prepared,
                ShortName = shortName,
                Name = name,
                TaxIdentificationNumber = taxIdentificationNumber
            };

            _logger.LogInformation($"Checking if facility with TIN = {taxIdentificationNumber} already exists.");
            await facility.CheckAlreadyExistsAsync(_facilityRepository);

            _logger.LogInformation($"Checking facility's TIN with VAT Register.");
            await facility.CheckAlreadyExistsAsync(_facilityRepository);

            facility.Profile= profile;
            _facilityRepository.Create(facility);
            await _db.Save();

            _facilityRepository.CreateEvent(new FacilityEvent
            {
                TypeId = FacilityEventType.Type.Created,
                Facility = facility,
                AddInfo = facility.ToString(),
                CallerProfile = profile
            });
            await _db.Save();

            return facility;
        }
    }
}
