using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByAddress;

public class GetByAddressQueryHandler : IRequestHandler<GetByAddressQuery, PagedList<AppointmentDto>>
{
    private readonly ILogger<GetByAddressQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IFacilityRepository _facilityRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public GetByAddressQueryHandler(
        ILogger<GetByAddressQueryHandler> logger,
        IMapper mapper,
        IAppointmentRepository appointmentRepository,
        IFacilityRepository facilityRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
        _facilityRepository = facilityRepository;
    }


    public async Task<PagedList<AppointmentDto>> Handle(GetByAddressQuery query, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Fetching address with id = {query.AddressId}.");
        var address = await new Address().GetByUUIDAsync(_facilityRepository, query.AddressId);

        var appointments = await _appointmentRepository.GetByAddressAsync(address);
        return PagedList<AppointmentDto>.ToPagedList(
            appointments.Select(s => _mapper.Map<AppointmentDto>(s)).ToList(),
            query.PagingParameters.PageNumber,
            query.PagingParameters.PageSize
        );
    }
}
