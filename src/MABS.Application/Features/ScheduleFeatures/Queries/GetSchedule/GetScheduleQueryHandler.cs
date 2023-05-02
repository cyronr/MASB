using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Application.ModelsExtensions.DoctorModelsExtensions;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.ScheduleFeatures.Queries.GetSchedule;

public class GetScheduleQueryHandler : IRequestHandler<GetScheduleQuery, List<ScheduleDto>>
{
    private readonly ILogger<GetScheduleQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IFacilityRepository _facilityRepository;
    private readonly IDoctorRepository _doctorRepository;

    public GetScheduleQueryHandler(
        ILogger<GetScheduleQueryHandler> logger, 
        IMapper mapper, 
        IScheduleRepository scheduleRepository, 
        IFacilityRepository facilityRepository, 
        IDoctorRepository doctorRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _scheduleRepository = scheduleRepository;
        _facilityRepository = facilityRepository;
        _doctorRepository = doctorRepository;
    }

    public async Task<List<ScheduleDto>> Handle(GetScheduleQuery query, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Fetching facility with id = {query.AddressId}.");
        var address = await new Address().GetByUUIDAsync(_facilityRepository, query.AddressId);

        _logger.LogDebug($"Fetching doctor with id = {query.DoctorId}.");
        var doctor = await new Doctor().GetByUUIDAsync(_doctorRepository, query.DoctorId);

        var schedules = await _scheduleRepository.GetByDoctorAndAddressAsync(doctor, address);
        return schedules.Select(s => _mapper.Map<ScheduleDto>(s)).ToList();
    }
}
