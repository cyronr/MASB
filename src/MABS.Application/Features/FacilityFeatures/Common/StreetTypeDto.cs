﻿using System.Text.Json;

namespace MABS.Application.Features.FacilityFeatures.Common
{
    public record StreetTypeDto
    {
        public string ShortName { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
