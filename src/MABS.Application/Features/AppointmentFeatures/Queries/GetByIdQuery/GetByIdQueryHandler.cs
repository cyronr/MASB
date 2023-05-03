using AutoMapper;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.ModelsExtensions.AppointmentModelsExtensions;
using MABS.Application.ModelsExtensions.ScheduleModelsExtensions;
using MABS.Domain.Models.AppointmentModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByIdQuery;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, AppointmentDto>
{
    private readonly ILogger<GetByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _appointmentRepository;
    public GetByIdQueryHandler(ILogger<GetByIdQueryHandler> logger, IMapper mapper, IAppointmentRepository appointmentRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
    }


    public async Task<AppointmentDto> Handle(GetByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Fetching Appointment with id = {query.Id}.");
        var patient = await new Appointment().GetByUUIDAsync(_appointmentRepository, query.Id);

        return _mapper.Map<AppointmentDto>(patient);
    }
}
