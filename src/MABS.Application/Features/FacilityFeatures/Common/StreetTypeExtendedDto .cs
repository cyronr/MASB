﻿using System.Text.Json;

namespace MABS.Application.Features.FacilityFeatures.Common
{
    public record StreetTypeExtendedDto
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
