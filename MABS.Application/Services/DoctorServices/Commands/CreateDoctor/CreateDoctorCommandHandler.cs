using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.DoctorServices.Common;
using MABS.Application.ServicesExtensions.DoctorServiceExtensions;
using MABS.Domain.Models.DoctorModels;

namespace MABS.Application.Services.DoctorServices.Commands.CreateDoctor
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, DoctorDto>
    {
        private readonly ILogger<CreateDoctorCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;

        public CreateDoctorCommandHandler(
            ILogger<CreateDoctorCommandHandler> logger, 
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

        public async Task<DoctorDto> Handle(CreateDoctorCommand command, CancellationToken cancellationToken)
        {
            var loggedProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile);

            _logger.LogDebug($"Fetching title with id = {command.TitleId}.");
            var title = await new Title().GetByIdAsync(_doctorRepository, command.TitleId);

            var specialties = new List<Specialty>();
            foreach (int specialityId in command.Specialties)
            {
                _logger.LogDebug($"Fetching speciality with id = {specialityId}.");
                specialties.Add(await new Specialty().GetByIdsAsync(_doctorRepository, specialityId));
            }

            Doctor doctor = new Doctor();
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    doctor.Firstname = command.Firstname;
                    doctor.Lastname = command.Lastname;
                    doctor.StatusId = DoctorStatus.Status.Active;
                    doctor.UUID = Guid.NewGuid();
                    doctor.Title = title;
                    doctor.Specialties = specialties;

                    _doctorRepository.Create(doctor);
                    await _db.Save();

                    _doctorRepository.CreateEvent(new DoctorEvent
                    {
                        TypeId = DoctorEventType.Type.Created,
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
