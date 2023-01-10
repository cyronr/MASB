using AutoMapper;
using MABS.Application.Common.Exceptions;
using MABS.Application.DTOs.ProfileDtos;
using MABS.Domain.Models.ProfileModels;
using MABS.Application.Services.Helpers.ProfileHelpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Profile = MABS.Domain.Models.ProfileModels.Profile;
using Microsoft.Extensions.Logging;
using MABS.Application.Repositories;

namespace MABS.Application.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {
        private readonly ILogger<ProfileService> _logger;
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IProfileHelper _helper;
        //private readonly IConfiguration _config;

        public ProfileService(ILogger<ProfileService> logger, IProfileRepository profileRepository, IMapper mapper, IDbOperation dbOperation, IProfileHelper helper/*, IConfiguration config*/)
        {
            _logger = logger;
            _profileRepository = profileRepository;
            _mapper = mapper;
            _db = dbOperation;
            _helper = helper;
            //_config = config;
        }

        public async Task<ProfileDto> RegisterPatientProfile(RegisterPatientProfileDto request)
        {
            Profile profile = new Profile
            {
                Email= request.Email,
                PhoneNumber = request.PhoneNumber
            };

            await _helper.CheckProfileAlreadyExists(profile);

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
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
            var profile = await _helper.GetByEmail(request.Email);
            if (!VerifyPasswordHash(request.Password, profile.PasswordHash, profile.PasswordSalt))
                throw new WrongPasswordException("Wrong password.");

            return CreateJwtToken(profile);
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

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            _logger.LogInformation("Creating hash password.");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            _logger.LogInformation("Veryfing password with hash password.");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computerHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computerHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateJwtToken(Profile profile)
        {
            _logger.LogInformation($"Creating login Jwt Token for {profile.Email}.");

            List<Claim> claims = new List<Claim> 
            {
                new Claim(ClaimTypes.NameIdentifier, profile.UUID.ToString()),
                new Claim(ClaimTypes.Name, profile.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes("dupa_i_chuj_a_nie_apka"/*_config.GetSection("AppSettings:Token").Value)*/));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            var strToken = tokenHandler.WriteToken(token);
            _logger.LogInformation($"Created login Jwt Token for {profile.Email} ({strToken}).");

            return strToken;
        }
    }
}
