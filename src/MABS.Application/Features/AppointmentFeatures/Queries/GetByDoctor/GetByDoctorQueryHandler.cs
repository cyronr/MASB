using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctor;

public class GetByDoctorQueryHandler : IRequestHandler<GetByDoctorQuery, PagedList<AppointmentDto>>
{
    private readonly ILogger<GetByDoctorQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public GetByDoctorQueryHandler(
        ILogger<GetByDoctorQueryHandler> logger,
        IMapper mapper,
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
    }


    public async Task<PagedList<AppointmentDto>> Handle(GetByDoctorQuery query, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Fetching doctor with id = {query.DoctorId}.");
        var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, query.DoctorId);

        var appointments = await _appointmentRepository.GetByDoctorAsync(doctor);
        return PagedList<AppointmentDto>.ToPagedList(
            appointments.Select(s => _mapper.Map<AppointmentDto>(s)).ToList(),
            query.PagingParameters.PageNumber,
            query.PagingParameters.PageSize
        );
    }
}
