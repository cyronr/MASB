using AutoMapper;
using MABS.Application.Common.AppProfile;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.FacilityFeatures.Commands.RemoveDoctorFromFacility
{
    public class RemoveDoctorFromFacilityCommandHandler : IRequestHandler<RemoveDoctorFromFacilityCommand, PagedList<DoctorDto>>
    {
        private readonly ILogger<RemoveDoctorFromFacilityCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ICurrentLoggedProfile _currentLoggedProfile;

        public RemoveDoctorFromFacilityCommandHandler(
            ILogger<RemoveDoctorFromFacilityCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IFacilityRepository facilityRepository,
            ICurrentLoggedProfile currentLoggedProfile,
            IDoctorRepository doctorRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _facilityRepository = facilityRepository;
            _currentLoggedProfile = currentLoggedProfile;
            _doctorRepository = doctorRepository;
        }


        public async Task<PagedList<DoctorDto>> Handle(RemoveDoctorFromFacilityCommand command, CancellationToken cancellationToken)
        {
            var callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile);

            _logger.LogDebug($"Fetching facility with id = {command.FacilityId}.");
            var facility = await new Facility().GetWithDoctorsByUUIDAsync(_facilityRepository, command.FacilityId);

            if (facility.Doctors.FirstOrDefault(d => d.UUID == command.DoctorId) is null)
                throw new NotFoundException($"Ten lekarz nie jest przypisany do wybranej placówki.", $"DoctorId = {command.DoctorId}, FacilityId = {command.FacilityId}");

            _logger.LogDebug($"Fetching doctor with id = {command.DoctorId}.");
            var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, command.DoctorId);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    facility.Doctors.Remove(doctor);
                    await _db.Save();

                    _facilityRepository.CreateEvent(new FacilityEvent
                    {
                        TypeId = FacilityEventType.Type.Updated,
                        Facility = facility,
                        AddInfo = $"Removed doctor ({doctor.ToString()}).",
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
            };

            return PagedList<DoctorDto>.ToPagedList(
                facility.Doctors.Select(d => _mapper.Map<DoctorDto>(d)).ToList(),
                command.PagingParameters.PageNumber,
                command.PagingParameters.PageSize
            );
        }
    }
}
