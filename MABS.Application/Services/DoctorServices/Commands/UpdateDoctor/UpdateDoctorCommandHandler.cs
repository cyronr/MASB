using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.DoctorServices.Common;
using MABS.Application.ServicesExtensions.DoctorServiceExtensions;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.Services.DoctorServices.Commands.UpdateDoctor
{
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, DoctorDto>
    {
        private readonly ILogger<UpdateDoctorCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;

        public UpdateDoctorCommandHandler(
            ILogger<UpdateDoctorCommandHandler> logger, 
            IMapper mapper,
            IDbOperation db,
            IDoctorRepository doctorRepository,
            ICurrentLoggedProfile currentLoggedProfile)
        {
            _doctorRepository = doctorRepository;
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _currentLoggedProfile = currentLoggedProfile;
        }

        public async Task<DoctorDto> Handle(UpdateDoctorCommand command, CancellationToken cancellationToken)
        {
            var loggedProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile);

            _logger.LogDebug($"Fetching doctor with id = {command.Id}.");
            var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, command.Id);

            _logger.LogDebug($"Fetching title with id = {command.TitleId}.");
            var title = await new Title().GetByIdAsync(_doctorRepository, command.TitleId);

            var specialties = new List<Specialty>();
            foreach (int specialityId in command.Specialties)
            {
                _logger.LogDebug($"Fetching speciality with id = {specialityId}.");
                specialties.Add(await new Specialty().GetByIdsAsync(_doctorRepository, specialityId));
            }

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    doctor.Firstname = command.Firstname;
                    doctor.Lastname = command.Lastname;
                    doctor.Title = title;
                    doctor.Specialties = specialties;
                    await _db.Save();

                    _doctorRepository.CreateEvent(new DoctorEvent
                    {
                        TypeId = DoctorEventType.Type.Updated,
                        Doctor = doctor,
                        AddInfo = doctor.ToString(),
                        CallerProfile = loggedProfile.GetProfileEntity()
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

            return _mapper.Map<DoctorDto>(doctor);
        }
    }
}
