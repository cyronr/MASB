using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Application.ModelsExtensions.PatientModelsExtensions;
using MABS.Domain.Models.PatientModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByPatient;

public class GetByPatientQueryHandler : IRequestHandler<GetByPatientQuery, PagedList<AppointmentDto>>
{
    private readonly ILogger<GetByPatientQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IPatientRepository _patientRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public GetByPatientQueryHandler(
        ILogger<GetByPatientQueryHandler> logger,
        IMapper mapper,
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
    }


    public async Task<PagedList<AppointmentDto>> Handle(GetByPatientQuery query, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Fetching patient with id = {query.PatientId}.");
        var patient = await new Patient().GetByUUIDAsync(_patientRepository, query.PatientId);

        var appointments = await _appointmentRepository.GetByPatientAsync(patient);
        return PagedList<AppointmentDto>.ToPagedList(
            appointments.Select(s => _mapper.Map<AppointmentDto>(s)).ToList(),
            query.PagingParameters.PageNumber,
            query.PagingParameters.PageSize
        );
    }
}
