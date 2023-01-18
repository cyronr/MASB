using MABS.Application.Common.Pagination;
using MABS.Application.DTOs.DoctorDtos;
using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Exceptions;
using MABS.Application.CRUD;


namespace MABS.Application.Services.DoctorServices
{
    public class DoctorService : BaseService<DoctorService>, IDoctorService
    {
        private readonly IDoctorCRUD _doctorCRUD;

        public DoctorService(
            IServicesDependencyAggregate<DoctorService> aggregate,
            IDoctorCRUD doctorCRUD) : base(aggregate)
        {
            _doctorCRUD = doctorCRUD;
        }


        public async Task<DoctorDto> Create(CreateDoctorDto request)
        {
            var title = await _doctorCRUD.Reader.GetTitleByIdAsync(request.TitleId);
            var specialties = await _doctorCRUD.Reader.GetSpecialtiesByIdsAsync(request.Specialties);

            Doctor doctor;
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    doctor = _mapper.Map<Doctor>(request);
                    doctor.StatusId = DoctorStatus.Status.Active;
                    doctor.UUID = Guid.NewGuid();
                    doctor.Title = title;
                    doctor.Specialties = specialties;
 
                    await _doctorCRUD.Creator.CreateAsync(doctor, LoggedProfile);
                    await _doctorCRUD.Creator.CreateEventAsync(doctor, DoctorEventType.Type.Created, LoggedProfile, doctor.ToString());

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto> Update(UpdateDoctorDto request)
        {
            var doctor = await _doctorCRUD.Reader.GetByUUIDAsync(request.Id);
            var title = await _doctorCRUD.Reader.GetTitleByIdAsync(request.TitleId);
            var specialties = await _doctorCRUD.Reader.GetSpecialtiesByIdsAsync(request.Specialties);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    _mapper.Map(request, doctor);
                    doctor.Title = title;
                    doctor.Specialties = specialties;

                    await _doctorCRUD.Updater.UpdateAsync(doctor, LoggedProfile);
                    await _doctorCRUD.Creator.CreateEventAsync(doctor, DoctorEventType.Type.Updated, LoggedProfile, doctor.ToString());

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task Delete(Guid id)
        {
            var doctor = await _doctorCRUD.Reader.GetByUUIDAsync(id);
            
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await _doctorCRUD.Deleter.DeleteAsync(doctor, LoggedProfile);
                    await _doctorCRUD.Creator.CreateEventAsync(doctor, DoctorEventType.Type.Deleted, LoggedProfile, doctor.ToString());

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };
        }

        public async Task<PagedList<DoctorDto>> GetAll(PagingParameters pagingParameters)
        {
            var doctors = await _doctorCRUD.Reader.GetAllAsync();
            return PagedList<DoctorDto>.ToPagedList(
                doctors.Select(d => _mapper.Map<DoctorDto>(d)).ToList(),
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<PagedList<DoctorDto>> GetBySpecalties(List<int> ids, PagingParameters pagingParameters)
        {
            var specalties = await _doctorCRUD.Reader.GetSpecialtiesByIdsAsync(ids);
            if (specalties.Count == 0)
                throw new MustBeAtLeastOneException("Must specify at least one Specialty.");

            var doctors = await _doctorCRUD.Reader.GetBySpecaltiesAsync(ids);
            return PagedList<DoctorDto>.ToPagedList(
                doctors.Select(d => _mapper.Map<DoctorDto>(d)).ToList(),
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<DoctorDto> GetById(Guid id)
        {
            var doctor = await _doctorCRUD.Reader.GetByUUIDAsync(id);
            return _mapper.Map<DoctorDto>(doctor);
        }
        public async Task<List<SpecialityExtendedDto>> GetAllSpecalties()
        {
            var specialties = await _doctorCRUD.Reader.GetAllSpecialtiesAsync();
            return specialties.Select(s => _mapper.Map<SpecialityExtendedDto>(s)).ToList();
        }

        public async Task<List<TitleExtendedDto>> GetAllTitles()
        {
            var titles = await _doctorCRUD.Reader.GetAllTitlesAsync();
            return titles.Select(t => _mapper.Map<TitleExtendedDto>(t)).ToList();
        }
    }
}