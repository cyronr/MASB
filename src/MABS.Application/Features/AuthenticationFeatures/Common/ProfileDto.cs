﻿using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.Features.AuthenticationFeatures.Common
{
    public record ProfileDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ProfileType.Type ProfileType { get; set; }
    }
}
