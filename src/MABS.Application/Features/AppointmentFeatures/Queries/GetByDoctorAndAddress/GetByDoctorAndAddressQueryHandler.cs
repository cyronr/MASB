using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByDoctorAndAddress;

public class GetByDoctorAndAddressQueryHandler : IRequestHandler<GetByDoctorAndAddressQuery, PagedList<AppointmentDto>>
{
    private readonly ILogger<GetByDoctorAndAddressQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IFacilityRepository _facilityRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public GetByDoctorAndAddressQueryHandler(
        ILogger<GetByDoctorAndAddressQueryHandler> logger,
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


    public async Task<PagedList<AppointmentDto>> Handle(GetByDoctorAndAddressQuery query, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Fetching address with id = {query.AddressId}.");
        var address = await new Address().GetByUUIDAsync(_facilityRepository, query.AddressId);

        _logger.LogDebug($"Fetching doctor with id = {query.DoctorId}.");
        var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, query.DoctorId);

        var appointments = await _appointmentRepository.GetByDoctorAndAddressAsync(doctor, address);
        return PagedList<AppointmentDto>.ToPagedList(
           appointments.Select(s => _mapper.Map<AppointmentDto>(s)).ToList(),
           query.PagingParameters.PageNumber,
           query.PagingParameters.PageSize
       );
    }
}
