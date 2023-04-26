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
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Domain.Models.FacilityModels;
using MABS.Application.ModelsExtensions.PatientModelsExtensions;

namespace MABS.Application.Features.PatientFeatures.Commands.UpdatePatient
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, PatientDto>
    {
        private readonly ILogger<UpdatePatientCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IPatientRepository _patientRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;

        public UpdatePatientCommandHandler(
            ILogger<UpdatePatientCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IPatientRepository patientRepository,
            ICurrentLoggedProfile currentLoggedProfile)
        {
            _patientRepository = patientRepository;
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _currentLoggedProfile = currentLoggedProfile;
        }

        public async Task<PatientDto> Handle(UpdatePatientCommand command, CancellationToken cancellationToken)
        {
            var callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile);

            _logger.LogDebug($"Fetching patient with id = {command.Id}.");
            var patient = await new Patient().GetByUUIDAsync(_patientRepository, command.Id);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    patient.Firstname = command.Firstname;
                    patient.Lastname = command.Lastname;

                    await _db.Save();

                    _patientRepository.CreateEvent(new PatientEvent
                    {
                        TypeId = PatientEventType.Type.Created,
                        Patient = patient,
                        AddInfo = patient.ToString(),
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

            return _mapper.Map<PatientDto>(patient);
        }
    }
}
