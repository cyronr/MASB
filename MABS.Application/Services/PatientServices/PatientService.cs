using AutoMapper;
using MABS.Application.DTOs.PatientDtos;
using MABS.Domain.Models.PatientModels;
using MABS.Application.Services.Helpers.PatientHelpers;
using Microsoft.Extensions.Logging;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.DataAccess.Common;

namespace MABS.Application.Services.PatientServices
{
    public class PatientService : IPatientService
    {
        private readonly ILogger<PatientService> _logger;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IPatientHelper _patientHelper;

        public PatientService(ILogger<PatientService> logger, IPatientRepository PatientRepository, IMapper mapper, IDbOperation dbOperation, IPatientHelper PatientHelper)
        {
            _db = dbOperation;
            _mapper = mapper;
            _logger = logger;
            _patientRepository = PatientRepository;
            _patientHelper = PatientHelper;
        }


        public async Task<PatientDto> GetById(Guid id)
        {
            var patient = await _patientHelper.GetPatientByUUID(id);
            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> Create(CreatePatientDto request)
        {
            var patient = _mapper.Map<Patient>(request);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    patient.UUID = Guid.NewGuid();
                    patient.StatusId = PatientStatus.Status.Active;

                    ///TODO: zmienić na prawdziwe dane
                    patient.ProfileId = 1;

                    await DoCreate(patient);

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> Update(UpdatePatientDto request)
        {
            var patient = await _patientHelper.GetPatientByUUID(request.Id);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    patient.Firstname = request.Firstname;
                    patient.Lastname = request.Lastname;
                    await DoUpdate(patient);

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task Delete(Guid id)
        {
            var patient = await _patientHelper.GetPatientByUUID(id);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await DoDelete(patient);
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };
        }

        private async Task DoCreate(Patient patient)
        {
            _patientRepository.Create(patient);
            await _db.Save();

            _patientRepository.CreateEvent(new PatientEvent
            {
                TypeId = PatientEventType.Type.Created,
                Patient = patient,
                AddInfo = patient.ToString()
            });
            await _db.Save();
        }

        private async Task DoUpdate(Patient patient)
        {
            _patientRepository.CreateEvent(new PatientEvent
            {
                TypeId = PatientEventType.Type.Updated,
                Patient = patient,
                AddInfo = patient.ToString()
            });
            await _db.Save();
        }

        private async Task DoDelete(Patient patient)
        {
            patient.StatusId = PatientStatus.Status.Deleted;

            _patientRepository.CreateEvent(new PatientEvent
            {
                TypeId = PatientEventType.Type.Deleted,
                Patient = patient,
            });
            await _db.Save();
        }
    }
}
