using AutoMapper;
using MABS.API.Requests.FacilityRequests;
using MABS.API.Requests.PatientRequests;
using MABS.API.Requests.ScheduleRequests;
using MABS.API.Responses.FacilityResponses;
using MABS.Application.Features.AppointmentFeatures.Command.CreateAppointment;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.Features.AuthenticationFeatures.Common;
using MABS.Application.Features.DictionaryFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Commands.CreateDoctor;
using MABS.Application.Features.DoctorFeatures.Commands.UpdateDoctor;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacility;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Commands.UpdateFacility;
using MABS.Application.Features.FacilityFeatures.Commands.UpdateFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.Features.PatientFeatures.Commands.UpdatePatient;
using MABS.Application.Features.PatientFeatures.Common;
using MABS.Application.Features.ScheduleFeatures.Commands.CreateSchedule;
using MABS.Application.Features.ScheduleFeatures.Commands.UpdateSchedule;
using MABS.Application.Features.ScheduleFeatures.Common;
using MASB.API.Requests.AppointmentRequests;
using MASB.API.Requests.AppointmentResponses;
using MASB.API.Requests.AuthenticationResponses;
using MASB.API.Requests.DoctorRequests;
using MASB.API.Responses.DictionaryResponses;
using MASB.API.Responses.DoctorResponses;
using MASB.API.Responses.PatientResponses;
using MASB.API.Responses.ScheduleResponses;

namespace MASB.API.Common.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        ///Dictionary mappings
        CreateMap<CityDto, CityResponse>();
        CreateMap<CountryDto, CountryResponse>();
        CreateMap<StreetTypeExtendedDto, StreetTypeResponse>();

        ///Doctor mappings
        CreateMap<DoctorDto, DoctorResponse>();
        CreateMap<CreateDoctorRequest, CreateDoctorCommand>();
        CreateMap<UpdateDoctorRequest, UpdateDoctorCommand>();
        CreateMap<TitleExtendedDto, TitleResponse>();
        CreateMap<SpecialityExtendedDto, SpecialtyResponse>();

        ///Authentication mappings
        CreateMap<AuthenticationResultDto, AuthenticationResponse>();

        ///Facility mappings
        CreateMap<FacilityDto, FacilityResponse>();
        CreateMap<CreateFacilityRequest, CreateFacilityCommand>();
        CreateMap<UpdateFacilityRequest, UpdateFacilityCommand>();
        CreateMap<CreateAddressRequest, CreateFacilityAddressCommand>();
        CreateMap<UpdateAddressRequest, UpdateFacilityAddressCommand>();

        ///Patient mappings
        CreateMap<PatientDto, PatientResponse>();
        CreateMap<UpdatePatientRequest, UpdatePatientCommand>();

        ///Schedule mappings
        CreateMap<ScheduleDto, ScheduleResponse>();
        CreateMap<CreateScheduleRequest, CreateScheduleCommand>();
        CreateMap<UpdateScheduleRequest, UpdateScheduleCommand>();


        ///Appointments mappings
        CreateMap<AppointmentDto, AppointmentResponse>()
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Patient.UUID))
            .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Schedule.Doctor.UUID))
            .ForMember(dest => dest.FacilityId, opt => opt.MapFrom(src => src.Schedule.Facility.UUID));
        CreateMap<CreateAppointmentRequest, CreateAppointmentCommand>();
    }   
}