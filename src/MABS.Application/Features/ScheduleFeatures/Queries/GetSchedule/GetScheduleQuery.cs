﻿using MABS.Application.Features.ScheduleFeatures.Common;
using MediatR;

namespace MABS.Application.Features.ScheduleFeatures.Queries.GetSchedule;

public record GetScheduleQuery 
(
    Guid DoctorId,
    Guid AddressId
) : IRequest<List<ScheduleDto>>;
