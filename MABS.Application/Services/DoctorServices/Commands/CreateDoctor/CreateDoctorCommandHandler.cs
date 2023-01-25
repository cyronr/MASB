using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.DoctorServices.Common;
using MABS.Domain.Models.DoctorModels;
using Profile = MABS.Domain.Models.ProfileModels.Profile;
using System.Numerics;
using MABS.Domain.Models.ProfileModels;
using System;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.ProfileModelsExtensions;

namespace MABS.Application.Services.DoctorServices.Commands.CreateDoctor
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, DoctorDto>
    {
        private readonly ILogger<CreateDoctorCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;
        private readonly IProfileRepository _profileRepository;

        public CreateDoctorCommandHandler(
            ILogger<CreateDoctorCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IDoctorRepository doctorRepository,
            ICurrentLoggedProfile currentLoggedProfile,
            IProfileRepository profileRepository)
        {
            _doctorRepository = doctorRepository;
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _currentLoggedProfile = currentLoggedProfile;
            _profileRepository = profileRepository;
        }


        public async Task<DoctorDto> Handle(CreateDoctorCommand command, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Fetching title with id = {command.TitleId}.");
            var title = await new Title().GetByIdAsync(_doctorRepository, command.TitleId);

            var specialties = new List<Specialty>();
            foreach (int specialityId in command.Specialties)
            {
                _logger.LogDebug($"Fetching speciality with id = {specialityId}.");
                specialties.Add(await new Specialty().GetByIdsAsync(_doctorRepository, specialityId));
            }

            Profile profile;
            if (command.ProfileId is null)
                profile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();
            else
                profile = await new Profile().GetByUUIDAsync(_profileRepository, (Guid)command.ProfileId);

            Doctor doctor;
            if (!_db.IsActiveTransaction())
            {
                using (var tran = _db.BeginTransaction())
                {
                    try
                    {
                        doctor = await DoCreate(command.Firstname, command.Lastname, title, specialties, profile);
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
                doctor = await DoCreate(command.Firstname, command.Lastname, title, specialties, profile);

            return _mapper.Map<DoctorDto>(doctor);
        }


        private async Task<Doctor> DoCreate(
            string firstname,
            string lastname,
            Title title,
            List<Specialty> specialties,
            Profile profile)
        {
            Doctor doctor = new Doctor
            {
                UUID = Guid.NewGuid(),
                StatusId = DoctorStatus.Status.Active,
                Firstname = firstname,
                Lastname = lastname,
                Title = title,
                Specialties = specialties,
                Profile = profile
            };

            _doctorRepository.Create(doctor);
            await _db.Save();

            _doctorRepository.CreateEvent(new DoctorEvent
            {
                TypeId = DoctorEventType.Type.Created,
                Doctor = doctor,
                AddInfo = doctor.ToString(),
                CallerProfile = profile
            });
            await _db.Save();

            return doctor;
        }

    }
}
