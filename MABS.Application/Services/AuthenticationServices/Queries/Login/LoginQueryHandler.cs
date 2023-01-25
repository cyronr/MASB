
using AutoMapper;
using MABS.Application.Common.Authentication;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.ModelsExtensions.ProfileModelsExtensions;
using MABS.Application.Services.AuthenticationServices.Common;
using MABS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Profile = MABS.Domain.Models.ProfileModels.Profile;

namespace MABS.Application.Services.AuthenticationServices.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResultDto>
    {
        private readonly ILogger<LoginQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProfileRepository _profileRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginQueryHandler(
            ILogger<LoginQueryHandler> logger,
            IMapper mapper,
            IProfileRepository profileRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _profileRepository = profileRepository;
            _logger = logger;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthenticationResultDto> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Logging profile ${query.Email}.");

            _logger.LogDebug($"Fetching profile by email {query.Email}.");
            var profile = await new Profile().GetByEmailAsync(_profileRepository, query.Email);

            if (!_passwordHasher.VerifyPassword(query.Password, profile.PasswordHash, profile.PasswordSalt))
                throw new WrongPasswordException("Wrong password.");

            var token = _jwtTokenGenerator.GenerateToken(profile);

            return new AuthenticationResultDto(_mapper.Map<ProfileDto>(profile), token);
        }

    }
}
