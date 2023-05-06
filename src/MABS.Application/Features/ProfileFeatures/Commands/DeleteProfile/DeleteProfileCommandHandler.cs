using MABS.Application.Common.AppProfile;
using MABS.Application.Common.Pagination;
using MABS.Application.DataAccess.Common;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByAddress;
using MABS.Application.Features.AppointmentFeatures.Queries.GetByPatient;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Queries.GetFacilityByProfile;
using MABS.Application.Features.PatientFeatures.Common;
using MABS.Application.Features.PatientFeatures.Queries.GetPatientByProfile;
using MABS.Application.ModelsExtensions.FacilityModelsExtensions;
using MABS.Application.ModelsExtensions.PatientModelsExtensions;
using MABS.Application.ModelsExtensions.ProfileModelsExtensions;
using MABS.Domain.Exceptions;
using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ProfileModels;
using MABS.Domain.Models.ScheduleModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MABS.Application.Features.ProfileFeatures.Commands.DeleteProfile;

public class DeleteProfileCommandHandler : IRequestHandler<DeleteProfileCommand>
{
    private readonly ILogger<DeleteProfileCommandHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IDbOperation _db;
    private readonly ICurrentLoggedProfile _currentLoggedProfile;
    private readonly IProfileRepository _profileRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IFacilityRepository _facilityRepository;

    private readonly DateOnly currentDate;
    private readonly TimeOnly currentTime;

    private static FacilityDto profileFacility;
    private static PatientDto profilePatient;

    public DeleteProfileCommandHandler(
        ILogger<DeleteProfileCommandHandler> logger,
        IProfileRepository profileRepository,
        IDbOperation db,
        ICurrentLoggedProfile currentLoggedProfile,
        IMediator mediator,
        IPatientRepository patientRepository,
        IFacilityRepository facilityRepository)
    {
        _logger = logger;
        _profileRepository = profileRepository;
        _db = db;
        _currentLoggedProfile = currentLoggedProfile;
        _mediator = mediator;

        currentDate = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        currentTime = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute);
        _patientRepository = patientRepository;
        _facilityRepository = facilityRepository;
    }


    public async Task<Unit> Handle(DeleteProfileCommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Getting current logged profile.");
        var callerProfile = CallerProfile.GetCurrentLoggedProfile(_currentLoggedProfile).GetProfileEntity();

        _logger.LogDebug($"Getting profile with id = {command.ProfileId}");
        var profile = await new Profile().GetByUUIDAsync(_profileRepository, command.ProfileId);

        await PrepareData(profile);
        await CheckProfile(profile);

        using (var tran = _db.BeginTransaction())
        {
            try
            {
                profile.StatusId = ProfileStatus.Status.Deleted;
                await _db.Save();

                _profileRepository.CreateEvent(new ProfileEvent
                {
                    Profile = profile,
                    TypeId = ProfileEventType.Type.Deleted,
                    CallerProfile = callerProfile
                });
                await _db.Save();

                await CleanProfileData(profile, callerProfile);

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw;
            }
        }

        return await Task.FromResult(Unit.Value);
    }

    private async Task PrepareData(Profile profile)
    {
        if (profile.TypeId == ProfileType.Type.Facility)
            profileFacility = await _mediator.Send(new GetFacilityByProfileQuery(profile.UUID));
        else if (profile.TypeId == ProfileType.Type.Patient)
            profilePatient = await _mediator.Send(new GetPatientByProfileQuery(profile.UUID));
    }

    private async Task CheckProfile(Profile profile)
    {
        if (profile.TypeId == ProfileType.Type.Facility)
            await CheckFacilityProfile(profile);
        else if (profile.TypeId == ProfileType.Type.Patient)
            await CheckPatientProfile(profile);
    }

    private async Task CheckFacilityProfile(Profile profile)
    {
        foreach (var address in profileFacility.Addresses)
        {
            var appointmentsForAddress = await _mediator.Send(new GetByAddressQuery(address.Id, new PagingParameters { PageNumber =1, PageSize = 100 }));
            if (AreActiveAppointments(appointmentsForAddress))
                throw new ConflictException("Istnieją aktywne wiztyty dla adresów.");
        }
    }

    private async Task CheckPatientProfile(Profile profile)
    {
        var appointmentsForPatient = await _mediator.Send(new GetByPatientQuery(profilePatient.Id, new PagingParameters { PageNumber = 1, PageSize = 100 }));
        if (AreActiveAppointments(appointmentsForPatient))
            throw new ConflictException("Istnieją aktywne wiztyty dla adresów.");
    }

    private bool AreActiveAppointments(List<AppointmentDto> appointments)
    {
        return appointments.Any(a =>
                    a.Status != AppointmentStatus.Status.Cancelled &&
                    a.Date > currentDate &&
                    a.Time > currentTime);
    }

    private async Task CleanProfileData(Profile profile, Profile callerProfile)
    {
        if (profile.TypeId == ProfileType.Type.Facility)
            await CleanFacilityData(callerProfile);
        else if (profile.TypeId == ProfileType.Type.Patient) 
            await CleanPatientData(callerProfile);
    }

    private async Task CleanFacilityData(Profile callerProfile)
    {
        var facility = await new Facility().GetByUUIDAsync(_facilityRepository, profileFacility.Id);

        facility.StatusId = FacilityStatus.Status.Deleted;
        _facilityRepository.CreateEvent(new FacilityEvent
        {
            Facility = facility,
            TypeId = FacilityEventType.Type.Deleted,
            CallerProfile = callerProfile
        });

        foreach (var address in facility.Addresses)
        {
            address.StatusId = AddressStatus.Status.Deleted;
        }

        await _db.Save();
    }

    private async Task CleanPatientData(Profile callerProfile)
    {
        var patient = await new Patient().GetByUUIDAsync(_patientRepository, profilePatient.Id);

        patient.StatusId = PatientStatus.Status.Deleted;
        _patientRepository.CreateEvent(new PatientEvent
        {
            Patient = patient,
            TypeId = PatientEventType.Type.Deleted,
            CallerProfile = callerProfile
        });

        await _db.Save();
    }
}
