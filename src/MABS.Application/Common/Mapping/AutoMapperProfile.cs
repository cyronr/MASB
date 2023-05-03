using AutoMapper;
using MABS.Application.Features.AppointmentFeatures.Common;
using MABS.Application.Features.AuthenticationFeatures.Common;
using MABS.Application.Features.DictionaryFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Common;
using MABS.Application.Features.PatientFeatures.Common;
using MABS.Application.Features.ScheduleFeatures.Common;
using MABS.Domain.Models.AppointmentModels;
using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;
using MABS.Domain.Models.ScheduleModels;
using ProfileEntity = MABS.Domain.Models.ProfileModels.Profile;

namespace MABS.Application.Common.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ///Dictionaries
            CreateMap<City, CityDto>();

            ///Doctors
            CreateMap<Doctor, DoctorDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.StatusId));
            CreateMap<Specialty, SpecialityDto>();
            CreateMap<Specialty, SpecialityExtendedDto>();
            CreateMap<Title, TitleDto>();
            CreateMap<Title, TitleExtendedDto>();

            ///Facilities
            CreateMap<Facility, FacilityDto>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID));
            CreateMap<Facility, DoctorFacilityDto>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID));
            CreateMap<Address, AddressDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID));
            CreateMap<Address, DoctorAddressDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID));
            CreateMap<AddressStreetType, StreetTypeDto>();
            CreateMap<AddressStreetType, StreetTypeExtendedDto>();
            CreateMap<Country, CountryDto>();

            ///Patients
            CreateMap<Patient, PatientDto>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID));

            ///Profiles
            CreateMap<ProfileEntity, ProfileDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID))
                .ForMember(d => d.ProfileType, opt => opt.MapFrom(s => s.TypeId));

            ///Schedules
            CreateMap<Schedule, ScheduleDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.StatusId));

            ///Appointments
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.StatusId));
        }
    }
}