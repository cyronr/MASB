using AutoMapper;
using MABS.API.Requests.FacilityRequests;
using MABS.API.Responses.FacilityResponses;
using MABS.Application.Features.AuthenticationFeatures.Common;
using MABS.Application.Features.DictionaryFeatures.Common;
using MABS.Application.Features.DoctorFeatures.Commands.CreateDoctor;
using MABS.Application.Features.DoctorFeatures.Commands.UpdateDoctor;
using MABS.Application.Features.DoctorFeatures.Common;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacility;
using MABS.Application.Features.FacilityFeatures.Commands.CreateFacilityAddress;
using MABS.Application.Features.FacilityFeatures.Common;
using MASB.API.Requests.AuthenticationResponses;
using MASB.API.Requests.DoctorRequests;
using MASB.API.Responses.DictionaryResponses;
using MASB.API.Responses.DoctorResponses;

namespace MASB.API.Common.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ///Dictionary mappings
            CreateMap<CityDto, CityResponse>();

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
            //CreateMap<UpdateFacilityRequest, UpdateFacilityCommand>();
            CreateMap<CreateAddressRequest, CreateFacilityAddressCommand>();

        }
    }
}