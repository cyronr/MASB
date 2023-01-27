using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using Profile = MABS.Domain.Models.ProfileModels.Profile;
using MABS.Application.Services.PatientServices.Common;
using MABS.Domain.Models.PatientModels;
using MABS.Application.ModelsExtensions.ProfileModelsExtensions;

namespace MABS.Application.Services.PatientServices.Commands.CreatePatient
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, PatientDto>
    {
        private readonly ILogger<CreatePatientCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IPatientRepository _patientRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;
        private readonly IProfileRepository _profileRepository;

        public CreatePatientCommandHandler(
            ILogger<CreatePatientCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IPatientRepository patientRepository,
            ICurrentLoggedProfile currentLoggedProfile,
            IProfileRepository profileRepository)
        {
            _patientRepository = patientRepository;
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _currentLoggedProfile = currentLoggedProfile;
            _profileRepository = profileRepository;
        }

        public async Task<PatientDto> Handle(CreatePatientCommand command, CancellationToken cancellationToken)
        {
            Patient patient = new Patient();
            patient.Firstname = command.Firstname;
            patient.Lastname = command.Lastname;
            patient.StatusId = PatientStatus.Status.Active;
            patient.UUID = Guid.NewGuid();

            var profile = await new Profile().GetByUUIDAsync(_profileRepository, command.ProfileId);
            patient.Profile = profile;

            _patientRepository.Create(patient);
            await _db.Save();

            _patientRepository.CreateEvent(new PatientEvent
            {
                TypeId = PatientEventType.Type.Created,
                Patient = patient,
                AddInfo = patient.ToString(),
                CallerProfile = profile
            });
            await _db.Save();

            return _mapper.Map<PatientDto>(patient);
        }
    }
}
