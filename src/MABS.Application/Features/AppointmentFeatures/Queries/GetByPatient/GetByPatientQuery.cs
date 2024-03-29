﻿using MABS.Application.Common.Pagination;
using MABS.Application.Features.AppointmentFeatures.Common;
using MediatR;

namespace MABS.Application.Features.AppointmentFeatures.Queries.GetByPatient;

public record GetByPatientQuery(
    Guid PatientId,
    PagingParameters PagingParameters
) : IRequest<PagedList<AppointmentDto>>;

