﻿namespace MABS.API.Requests.ScheduleRequests;

public record CreateScheduleRequest : UpsertScheduleRequest
{
    public Guid DoctorId { get; set; }
    public Guid AddressId { get; set; }
}
