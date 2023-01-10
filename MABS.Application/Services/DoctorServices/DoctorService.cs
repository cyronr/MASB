using MABS.Application.Common.Pagination;
using MABS.Application.DTOs.DoctorDtos;
using MABS.Domain.Models.DoctorModels;
using MABS.Application.Services.Helpers;
using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Exceptions;

namespace MABS.Application.Services.DoctorServices
{
    public class DoctorService : BaseService<DoctorService>, IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IHelpers _helpers;

        public DoctorService(
            IServicesDependencyAggregate<DoctorService> aggregate,
            IDoctorRepository doctorRepository, 
            IHelpers helpers) :base(aggregate)
        {
            _doctorRepository = doctorRepository;
            _helpers = helpers;
        }


        public async Task<DoctorDto> Create(CreateDoctorDto request)
        {
            var profile = LoggedProfile;
            var title = await _helpers.Doctor.GetTitleById(request.TitleId);
            var specalties = await _helpers.Doctor.GetSpecialtiesByIds(request.Specialties);

            var doctor = _mapper.Map<Doctor>(request);
            doctor.StatusId = DoctorStatus.Status.Active;
            doctor.UUID = Guid.NewGuid();
            doctor.Title = title;
            doctor.Specialties = specalties;

            await DoCreate(doctor);

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto> Update(UpdateDoctorDto request)
        {
            var doctor = await _helpers.Doctor.GetDoctorByUUID(request.Id);
            var title = await _helpers.Doctor.GetTitleById(request.TitleId);
            var specalties = await _helpers.Doctor.GetSpecialtiesByIds(request.Specialties);

            _mapper.Map(request, doctor);
            doctor.Title = title;
            doctor.Specialties = specalties;
            
            await DoUpdate(doctor);

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task Delete(Guid id)
        {
            var doctor = await _helpers.Doctor.GetDoctorByUUID(id);
            doctor.StatusId = DoctorStatus.Status.Deleted;

            await DoDelete(doctor);
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
            var specalties = await _helpers.Doctor.GetSpecialtiesByIds(ids);
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
            var doctor = await _helpers.Doctor.GetDoctorByUUID(id);
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

        private async Task DoCreate(Doctor doctor)
        {
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    _doctorRepository.Create(doctor);
                    await _db.Save();

                    _doctorRepository.CreateEvent(new DoctorEvent
                    {
                        TypeId = DoctorEventType.Type.Created,
                        Doctor = doctor
                    });
                    await _db.Save();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;
                }
            };
        }

        private async Task DoUpdate(Doctor doctor)
        {
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    _doctorRepository.CreateEvent(new DoctorEvent
                    {
                        TypeId = DoctorEventType.Type.Updated,
                        Doctor = doctor
                    });
                    await _db.Save();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        private async Task DoDelete(Doctor doctor)
        {
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    _doctorRepository.CreateEvent(new DoctorEvent
                    {
                        TypeId = DoctorEventType.Type.Deleted,
                        Doctor = doctor
                    });
                    await _db.Save();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
    }
}