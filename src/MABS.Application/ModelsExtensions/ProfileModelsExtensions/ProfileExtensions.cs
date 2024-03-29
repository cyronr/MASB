﻿using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.ProfileModels;

namespace MABS.Application.ModelsExtensions.ProfileModelsExtensions
{
    public static class ProfileExtensions
    {
        public static async Task<Profile> GetByEmailAsync(this Profile profile, IProfileRepository profileRepository, string email)
        {
            profile = await profileRepository.GetByEmailAsync(email);
            if (profile is null)
                throw new NotFoundException($"Profile with email = {email} was not found.");

            return profile;
        }

        public static async Task<Profile> GetByUUIDAsync(this Profile profile, IProfileRepository profileRepository, Guid uuid)
        {
            profile = await profileRepository.GetByUUIDAsync(uuid);
            if (profile is null)
                throw new NotFoundException($"Profile with UUID = {uuid} was not found.");

            return profile;
        }

        public static async Task CheckAlreadyExists(this Profile profile, IProfileRepository profileRepository)
        {
            if (await profileRepository.GetByEmailAsync(profile.Email) is not null)
                throw new AlreadyExistsException($"Profile with Email {profile.Email} already exists.");
        }
    }
}
