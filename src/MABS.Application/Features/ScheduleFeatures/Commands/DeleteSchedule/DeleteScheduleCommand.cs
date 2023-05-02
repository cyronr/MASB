using MABS.Application.Features.ScheduleFeatures.Common;
using MediatR;

namespace MABS.Application.Features.ScheduleFeatures.Commands.DeleteSchedule;

public record DeleteScheduleCommand(
    Guid ScheduleId
) : IRequest<ScheduleDto>;
