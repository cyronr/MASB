using MABS.Application.Common.Pagination;
using MABS.Application.DTOs.DoctorDtos;

namespace MABS.Application.Services.DoctorServices
{
    public interface IDoctorService
    {
        Task<DoctorDto> GetById(Guid id);
        Task<PagedList<DoctorDto>> GetAll(PagingParameters pagingParameters);
        Task<PagedList<DoctorDto>> GetBySpecalties(List<int> ids, PagingParameters pagingParameters);
        Task<DoctorDto> Create(CreateDoctorDto request);
        Task<DoctorDto> Update(UpdateDoctorDto request);
        Task Delete(Guid id);
        Task<List<SpecialityExtendedDto>> GetAllSpecalties();
        Task<List<TitleExtendedDto>> GetAllTitles();
    }
}