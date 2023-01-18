using MABS.Application.DTOs.ProfileDtos;
using MABS.Domain.Models.ProfileModels;
using Profile = MABS.Domain.Models.ProfileModels.Profile;
using MABS.Domain.Exceptions;
using MABS.Application.Common.Authentication;
using MABS.Application.CRUD;
using MABS.Application.Checkers.ProfileCheckers;

namespace MABS.Application.Services.AuthenticationServices
{
    public class AuthenticationService : BaseService<AuthenticationService>, IAuthenticationService
    {
        private readonly IProfileCRUD _profileCRUD;
        private readonly IProfileChecker _profileChecker;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(
            IServicesDependencyAggregate<AuthenticationService> aggregate,
            IProfileCRUD profileCRUD,
            IProfileChecker profileChecker,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher passwordHasher) : base(aggregate)
        {
            _profileCRUD = profileCRUD;
            _profileChecker = profileChecker;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<ProfileDto> RegisterPatientProfile(RegisterPatientProfileDto request)
        {
            Profile profile = new Profile
            {
                Email= request.Email,
                PhoneNumber = request.PhoneNumber
            };

            await _profileChecker.CheckProfileAlreadyExistsAsync(profile);

            _passwordHasher.GeneratePassword(request.Password, out byte[] passwordHasher, out byte[] passwordSalt);
            profile.PasswordHash = passwordHasher;
            profile.PasswordSalt = passwordSalt;
            profile.UUID = Guid.NewGuid();
            profile.StatusId = ProfileStatus.Status.Prepared;
            profile.TypeId = ProfileType.Type.Patient;

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await _profileCRUD.Creator.CreateAsync(profile, LoggedProfile);
                    await _profileCRUD.Creator.CreateEventAsync(profile, ProfileEventType.Type.Created, LoggedProfile, profile.ToString());

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
            var profile = await _profileCRUD.Reader.GetByEmailAsync(request.Email);
            if (!_passwordHasher.VerifyPassword(request.Password, profile.PasswordHash, profile.PasswordSalt))
                throw new WrongPasswordException("Wrong password.");

            return _jwtTokenGenerator.GenerateToken(profile);
        }
    }
}
