using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Models.ProfileModels;
using MABS.Application.Common.Authentication;
using Profile = MABS.Domain.Models.ProfileModels.Profile;
using MABS.Application.ModelsExtensions.ProfileModelsExtensions;
using MABS.Application.Features.AuthenticationFeatures.Common;
using MABS.Application.Common.MessageSenders;
using System.Net.Mail;
using MABS.Application.Features.InternalFeatures.Notifications.SendEmail;

namespace MABS.Application.Features.AuthenticationFeatures.Commands.RegisterPatient
{
    public class RegisterPatientCommandHandler : IRequestHandler<RegisterPatientCommand, AuthenticationResultDto>
    {
        private readonly ILogger<RegisterPatientCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IProfileRepository _profileRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMediator _mediator;

        public RegisterPatientCommandHandler(
            ILogger<RegisterPatientCommandHandler> logger,
            IMapper mapper,
            IDbOperation db,
            IProfileRepository profileRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IMediator mediator,
            IEmailSender emailSender)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
            _profileRepository = profileRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mediator = mediator;
        }

        public async Task<AuthenticationResultDto> Handle(RegisterPatientCommand command, CancellationToken cancellationToken)
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
                    profile.TypeId = ProfileType.Type.Patient;

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

                    command.Patient.ProfileId = profile.UUID;
                    await _mediator.Send(command.Patient);

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            await SendRegistrationEmail(profile);
            var token = _jwtTokenGenerator.GenerateToken(profile);
            return new AuthenticationResultDto(_mapper.Map<ProfileDto>(profile), token);
        }

        private async Task SendRegistrationEmail(Profile profile)
        {
            PrepareRegistrationEmail(profile, out string subject, out string body);
            await _mediator.Send(new SendEmailCommand(subject, body, profile.Email));
        }

        private void PrepareRegistrationEmail(Profile profile, out string subject, out string body)
        {
            subject = "Nowe konto w serwisie MediReserve.";
            body = @$"
            Witaj {profile.Patient.Firstname} {profile.Patient.Lastname}! <br />
            <br />
            Potwierdzamy stworzenie nowego konta w serwisie MediReserve. <br />
            <br />
            Pozdrowienia, <br />
            Zespół MediReserve :) <br />
            ";
        }
    }
}
