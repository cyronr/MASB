﻿namespace MASB.API.Responses.DoctorResponses
{
    public record TitleResponse
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
    }
}
