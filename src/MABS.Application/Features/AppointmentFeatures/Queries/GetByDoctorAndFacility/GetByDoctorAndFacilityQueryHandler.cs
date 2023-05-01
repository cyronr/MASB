using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctorAndFacility;

public class GetByDoctorAndFacilityQueryHandler : IRequestHandler<GetByDoctorAndFacilityQuery, List<AppointmentDto>>
{
    private readonly ILogger<GetByDoctorAndFacilityQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IFacilityRepository _facilityRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public GetByDoctorAndFacilityQueryHandler(
        ILogger<GetByDoctorAndFacilityQueryHandler> logger,
        IMapper mapper,
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        IFacilityRepository facilityRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
        _facilityRepository = facilityRepository;
    }


    public async Task<List<AppointmentDto>> Handle(GetByDoctorAndFacilityQuery query, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Fetching facility with id = {query.FacilityId}.");
        var facility = await new Facility().GetByUUIDAsync(_facilityRepository, query.FacilityId);

        _logger.LogDebug($"Fetching doctor with id = {query.DoctorId}.");
        var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, query.DoctorId);

        var schedules = await _appointmentRepository.GetByDoctorAndFacilityAsync(doctor, facility);
        return schedules.Select(s => _mapper.Map<AppointmentDto>(s)).ToList();
    }
}
