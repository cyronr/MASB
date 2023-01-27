using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MABS.Application.Common.AppProfile;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.DoctorServices.Common;
using MABS.Domain.Models.DoctorModels;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;

namespace MABS.Application.Services.DoctorServices.Commands.DeleteDoctor
{
    public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, DoctorDto>
    {
        private readonly ILogger<DeleteDoctorCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;

        public DeleteDoctorCommandHandler(
            ILogger<DeleteDoctorCommandHandler> logger, 
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

        public async Task<DoctorDto> Handle(DeleteDoctorCommand command, CancellationToken cancellationToken)
        {
            var loggedProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile);

            _logger.LogDebug($"Fetching doctor with id = {command.Id}.");
            var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, command.Id);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    doctor.StatusId = DoctorStatus.Status.Deleted;
                    await _db.Save();

                    _doctorRepository.CreateEvent(new DoctorEvent
                    {
                        TypeId = DoctorEventType.Type.Deleted,
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
