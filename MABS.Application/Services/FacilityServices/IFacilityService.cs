using MABS.Application.Common.Pagination;
using MABS.Application.DTOs.DoctorDtos;
using MABS.Application.DTOs.FacilityDtos;

namespace MABS.Application.Services.FacilityServices
{
    public interface IFacilityService
    {
        Task<FacilityDto> GetById(Guid id);
        Task<PagedList<FacilityDto>> GetAll(PagingParameters pagingParameters);
        Task<FacilityDto> Create(CreateFacilityDto request);
        Task<FacilityDto> Update(UpdateFacilityDto request);
        Task Delete(Guid id);
        Task<FacilityDto> CreateAddress(Guid facilityId, CreateAddressDto request);
        Task<FacilityDto> UpdateAddress(Guid facilityId, UpdateAddressDto request);
        Task<FacilityDto> DeleteAddress(Guid facilityId, Guid addressId);
        Task<PagedList<DoctorDto>> GetAllDoctors(PagingParameters pagingParameters, Guid facilityId);
        Task<PagedList<DoctorDto>> AddDoctor(PagingParameters pagingParameters, Guid facilityId, Guid doctorId);
        Task<PagedList<DoctorDto>> RemoveDoctor(PagingParameters pagingParameters, Guid facilityId, Guid doctorId);
        Task<List<CountryDto>> GetAllCountries();
        Task<List<StreetTypeExtendedDto>> GetAllStreetTypes();

    }
}