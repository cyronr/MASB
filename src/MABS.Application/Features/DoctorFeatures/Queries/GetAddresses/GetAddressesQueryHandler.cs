using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.ScheduleModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.DoctorFeatures.Queries.GetAddresses;

public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, List<DoctorAddressDto>>
{
    private readonly ILogger<GetAddressesQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IFacilityRepository _facilityRepository;

    public GetAddressesQueryHandler(
        ILogger<GetAddressesQueryHandler> logger,
        IMapper mapper, 
        IDoctorRepository doctorRepository, 
        IFacilityRepository facilityRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _doctorRepository = doctorRepository;
        _facilityRepository = facilityRepository;
    }


    public async Task<List<DoctorAddressDto>> Handle(GetAddressesQuery query, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Fetching doctor with id = {query.DoctorId}.");
        var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, query.DoctorId);

        var addresses = await _facilityRepository.GetAddressesByDoctorAsync(doctor);
        return addresses.Select(s => _mapper.Map<DoctorAddressDto>(s)).ToList();
    }
}
