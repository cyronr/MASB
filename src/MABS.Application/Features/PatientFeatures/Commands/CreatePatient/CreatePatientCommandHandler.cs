using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using Profile = MABS.Domain.Models.ProfileModels.Profile;
using MABS.Domain.Models.PatientModels;
using MABS.Application.ModelsExtensions.ProfileModelsExtensions;
using MABS.Application.Features.PatientFeatures.Common;
using MABS.Domain.Models.DoctorModels;
using System.Numerics;
using MABS.Application.Features.DoctorFeatures.Common;

namespace MABS.Application.Features.PatientFeatures.Commands.CreatePatient
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
            Profile profile;
            if (command.ProfileId is null)
                profile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();
            else
                profile = await new Profile().GetByUUIDAsync(_profileRepository, (Guid)command.ProfileId);

            Patient patient;
            if (!_db.IsActiveTransaction())
            {
                using (var tran = _db.BeginTransaction())
                {
                    try
                    {
                        patient = await DoCreate(command.Firstname, command.Lastname, profile);
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
                patient = await DoCreate(command.Firstname, command.Lastname, profile);

            return _mapper.Map<PatientDto>(patient);
        }

        private async Task<Patient> DoCreate(
            string firstname,
            string lastname,
            Profile profile)
        {
            Patient patient = new Patient
            {
                UUID = Guid.NewGuid(),
                StatusId = PatientStatus.Status.Active,
                Firstname = firstname,
                Lastname = lastname,
                Profile = profile
            };

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

            return patient;
        }
    }
}
