using MABS.Application.Common.Pagination;
using MABS.Application.DTOs.DoctorDtos;
using MABS.Domain.Models.DoctorModels;
using MABS.Application.Services.Helpers;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;
using MABS.Application.CRUD.Creators.DoctorCreator;
using MABS.Application.CRUD;

namespace MABS.Application.Services.DoctorServices
{
    public class DoctorService : BaseService<DoctorService>, IDoctorService
    {
        private readonly IDoctorCRUD _doctorCRUD;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(
            IServicesDependencyAggregate<DoctorService> aggregate,
            IDoctorCRUD doctorCRUD,
            IDoctorRepository doctorRepository) : base(aggregate)
        {
            _doctorCRUD = doctorCRUD;
            _doctorRepository = doctorRepository;
        }


        public async Task<DoctorDto> Create(CreateDoctorDto request)
        {
            var title = await _doctorCRUD.Reader.GetTitleById(request.TitleId);
            var specialties = await _doctorCRUD.Reader.GetSpecialtiesByIds(request.Specialties);

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
 
                    await _doctorCRUD.Creator.CreateDoctor(doctor, LoggedProfile);

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
            var doctor = await _doctorCRUD.Reader.GetDoctorByUUID(request.Id);
            var title = await _doctorCRUD.Reader.GetTitleById(request.TitleId);
            var specialties = await _doctorCRUD.Reader.GetSpecialtiesByIds(request.Specialties);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    _mapper.Map(request, doctor);
                    doctor.Title = title;
                    doctor.Specialties = specialties;

                    await _doctorCRUD.Updater.UpdateDoctor(doctor, LoggedProfile);

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
            var doctor = await _doctorCRUD.Reader.GetDoctorByUUID(id);
            
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await _doctorCRUD.Deleter.DeleteDoctor(doctor, LoggedProfile);

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
            var doctors = await _doctorRepository.GetAll();
            return PagedList<DoctorDto>.ToPagedList(
                doctors.Select(d => _mapper.Map<DoctorDto>(d)).ToList(),
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<PagedList<DoctorDto>> GetBySpecalties(List<int> ids, PagingParameters pagingParameters)
        {
            var specalties = await _doctorCRUD.Reader.GetSpecialtiesByIds(ids);
            if (specalties.Count == 0)
                throw new MustBeAtLeastOneException("Must specify at least one Specialty.");

            var doctors = await _doctorRepository.GetBySpecalties(ids);
            return PagedList<DoctorDto>.ToPagedList(
                doctors.Select(d => _mapper.Map<DoctorDto>(d)).ToList(),
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<DoctorDto> GetById(Guid id)
        {
            var doctor = await _doctorCRUD.Reader.GetDoctorByUUID(id);
            return _mapper.Map<DoctorDto>(doctor);
        }
        public async Task<List<SpecialityExtendedDto>> GetAllSpecalties()
        {
            var specialties = await _doctorRepository.GetAllSpecialties();
            return specialties.Select(s => _mapper.Map<SpecialityExtendedDto>(s)).ToList();
        }

        public async Task<List<TitleExtendedDto>> GetAllTitles()
        {
            var titles = await _doctorRepository.GetAllTitles();
            return titles.Select(t => _mapper.Map<TitleExtendedDto>(t)).ToList();
        }
    }
}