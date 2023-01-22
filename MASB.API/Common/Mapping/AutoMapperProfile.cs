using AutoMapper;
using MABS.API.Requests.AuthenticationRequests;
using MABS.Application.Services.AuthenticationServices.Common;
using MABS.Application.Services.AuthenticationServices.RegisterDoctor;
using MABS.Application.Services.DoctorServices.Commands.CreateDoctor;
using MABS.Application.Services.DoctorServices.Commands.UpdateDoctor;
using MABS.Application.Services.DoctorServices.Common;
using MASB.API.Requests.AuthenticationResponses;
using MASB.API.Requests.DoctorRequests;
using MASB.API.Responses.DoctorResponses;

namespace MASB.API.Common.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ///Doctor mappings
            CreateMap<DoctorDto, DoctorResponse>();
            CreateMap<CreateDoctorRequest, CreateDoctorCommand>();
            CreateMap<UpdateDoctorRequest, UpdateDoctorCommand>();
            CreateMap<TitleExtendedDto, TitleResponse>();
            CreateMap<SpecialityExtendedDto, SpecialtyResponse>();

            //Authentication mappings
            CreateMap<AuthenticationResultDto, AuthenticationResponse>();

        }
    }
}