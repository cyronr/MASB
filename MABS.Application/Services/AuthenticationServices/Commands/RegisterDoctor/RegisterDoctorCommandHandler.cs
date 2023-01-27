using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Services.AuthenticationServices.Common;
using MABS.Domain.Models.ProfileModels;
using MABS.Application.Common.Authentication;
using Profile = MABS.Domain.Models.ProfileModels.Profile;
using MABS.Application.ModelsExtensions.ProfileModelsExtensions;

namespace MABS.Application.Services.AuthenticationServices.Commands.RegisterDoctor
{
    public class RegisterDoctorCommandHandler : IRequestHandler<RegisterDoctorCommand, AuthenticationResultDto>
    {
        private readonly ILogger<RegisterDoctorCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IProfileRepository _profileRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMediator _mediator;

        public RegisterDoctorCommandHandler(
            ILogger<RegisterDoctorCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IProfileRepository profileRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _profileRepository = profileRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mediator = mediator;
        }

        public async Task<AuthenticationResultDto> Handle(RegisterDoctorCommand command, CancellationToken cancellationToken)
        {
            Profile profile = new Profile();
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    profile.Email = command.Email;
                    profile.PhoneNumber = command.PhoneNumber;

                    await profile.CheckAlreadyExists(_profileRepository);

                    profile.UUID = Guid.NewGuid();
                    profile.StatusId = ProfileStatus.Status.Prepared;
                    profile.TypeId = ProfileType.Type.Doctor;

                    _passwordHasher.GeneratePassword(command.Password, out byte[] passwordHasher, out byte[] passwordSalt);
                    profile.PasswordHash = passwordHasher;
                    profile.PasswordSalt = passwordSalt;

                    _profileRepository.Create(profile);
                    await _db.Save();

                    _profileRepository.CreateEvent(new ProfileEvent
                    {
                        TypeId = ProfileEventType.Type.Created,
                        Profile = profile,
                        AddInfo = profile.ToString(),
                        CallerProfile = profile
                    });
                    await _db.Save();

                    command.Doctor.ProfileId = profile.UUID;
                    await _mediator.Send(command.Doctor);

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            var token = _jwtTokenGenerator.GenerateToken(profile);
            return new AuthenticationResultDto(_mapper.Map<ProfileDto>(profile), token);
        }
    }
}
