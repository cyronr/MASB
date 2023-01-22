using AutoMapper;
using MABS.Application.DTOs.FacilityDtos;
using MABS.Application.DTOs.PatientDtos;
using MABS.Application.Services.AuthenticationServices.Common;
using MABS.Application.Services.DoctorServices.Common;
using MABS.Domain.Models.DictionaryModels;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using MABS.Domain.Models.PatientModels;
using ProfileEntity = MABS.Domain.Models.ProfileModels.Profile;

namespace MABS.Application.Common.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ///Doctors
            CreateMap<Doctor, DoctorDto>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID));
            CreateMap<Specialty, SpecialityDto>();
            CreateMap<Specialty, SpecialityExtendedDto>();
            CreateMap<Title, TitleDto>();
            CreateMap<Title, TitleExtendedDto>();

            ///Facilities
            CreateMap<Facility, FacilityDto>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID));
            CreateMap<Address, AddressDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID));
            CreateMap<AddressStreetType, StreetTypeDto>();
            CreateMap<AddressStreetType, StreetTypeExtendedDto>();
            CreateMap<CreateFacilityDto, Facility>();
            CreateMap<UpdateFacilityDto, Facility>()
                .ForMember(d => d.UUID, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Id, opt => opt.Ignore()); 
            CreateMap<CreateAddressDto, Address>();
            CreateMap<UpdateAddressDto, Address>()
                .ForMember(d => d.UUID, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Id, opt => opt.Ignore());
            CreateMap<Country, CountryDto>();

            ///Patients
            CreateMap<Patient, PatientDto>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID));
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<UpdatePatientDto, Patient>()
                .ForMember(d => d.UUID, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Id, opt => opt.Ignore());

            ///Profiles
            CreateMap<ProfileEntity, ProfileDto>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.UUID));

        }
    }
}