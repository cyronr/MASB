using AutoMapper;
using MABS.Application.DTOs.ProfileDtos;
using MABS.Domain.Models.ProfileModels;
using MABS.Application.Services.Helpers.ProfileHelpers;
using Profile = MABS.Domain.Models.ProfileModels.Profile;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Application.Common.Interfaces.Authentication;
using MABS.Application.Services.Helpers;

namespace MABS.Application.Services.AuthenticationServices
{
    public class AuthenticationService : BaseService<AuthenticationService>, IAuthenticationService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IHelpers _helpers;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHash;

        public AuthenticationService(
            IServicesDependencyAggregate<AuthenticationService> aggregate,
            IProfileRepository profileRepository,
            IHelpers helpers,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher passwordHash) : base(aggregate)
        {
            _profileRepository = profileRepository;
            _helpers = helpers;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHash = passwordHash;
        }

        public async Task<ProfileDto> RegisterPatientProfile(RegisterPatientProfileDto request)
        {
            Profile profile = new Profile
            {
                Email= request.Email,
                PhoneNumber = request.PhoneNumber
            };

            await _helpers.Profile.CheckProfileAlreadyExists(profile);

            _passwordHash.GeneratePassword(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            profile.PasswordHash = passwordHash;
            profile.PasswordSalt = passwordSalt;
            profile.UUID = Guid.NewGuid();
            profile.StatusId = ProfileStatus.Status.Prepared;
            profile.TypeId = ProfileType.Type.Patient;

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await DoCreate(profile);

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return _mapper.Map<ProfileDto>(profile);
        }

        public async Task<string> Login(LoginProfileDto request)
        {
            var profile = await _helpers.Profile.GetByEmail(request.Email);
            if (!_passwordHash.VerifyPassword(request.Password, profile.PasswordHash, profile.PasswordSalt))
                throw new WrongPasswordException("Wrong password.");

            return _jwtTokenGenerator.GenerateToken(profile);
        }

        private async Task DoCreate(Profile profile)
        {
            _profileRepository.Create(profile);
            await _db.Save();

            _profileRepository.CreateEvent(new ProfileEvent
            {
                TypeId = ProfileEventType.Type.Created,
                Profile = profile,
                AddInfo = profile.ToString()
            });
            await _db.Save();
        }
    }
}
