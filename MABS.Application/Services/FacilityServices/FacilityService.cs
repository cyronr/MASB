using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.DTOs.DoctorDtos;
using MABS.Application.DTOs.FacilityDtos;
using MABS.Domain.Models.FacilityModels;
using System.Net;
using Microsoft.Extensions.Logging;
using MABS.Domain.Exceptions;
using MABS.Application.CRUD;
using MABS.Application.CRUD.Readers.DoctorReaders;
using MABS.Domain.Models.DoctorModels;
using MABS.Application.Checkers.FacilityCheckers;
using System.Numerics;

namespace MABS.Application.Services.FacilityServices
{
    public class FacilityService : BaseService<FacilityService>, IFacilityService
    {
        private readonly IFacilityCRUD _facilityCRUD;
        private readonly IFacilityChecker _facilityChecker;
        private readonly IDoctorReader _doctorReader;

        public FacilityService(
            IServicesDependencyAggregate<FacilityService> aggregate,
            IFacilityCRUD facilityCRUD,
            IFacilityChecker facilityChecker,
            IDoctorReader doctorReader) : base(aggregate)
        {
            _facilityCRUD = facilityCRUD;
            _facilityChecker = facilityChecker;
            _doctorReader = doctorReader;
        }

        public async Task<PagedList<FacilityDto>> GetAll(PagingParameters pagingParameters)
        {
            var facilities = await _facilityCRUD.Reader.GetAllAsync();
            return PagedList<FacilityDto>.ToPagedList(
                facilities.Select(f => _mapper.Map<FacilityDto>(f)).ToList(),
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<FacilityDto> GetById(Guid id)
        {
            var facility = await _facilityCRUD.Reader.GetByUUIDAsync(id);
            return _mapper.Map<FacilityDto>(facility);
        }

        public async Task<List<CountryDto>> GetAllCountries()
        {
            var countries = await _facilityCRUD.Reader.GetAllCountriesAsync();
            return countries.Select(c => _mapper.Map<CountryDto>(c)).ToList();
        }

        public async Task<List<StreetTypeExtendedDto>> GetAllStreetTypes()
        {
            var streetTypes = await _facilityCRUD.Reader.GetAllStreetTypesAsync();
            return streetTypes.Select(c => _mapper.Map<StreetTypeExtendedDto>(c)).ToList();
        }
        public async Task<PagedList<DoctorDto>> GetAllDoctors(PagingParameters pagingParameters, Guid facilityId)
        {
            var facility = await _facilityCRUD.Reader.GetFacilityWithDoctorsByUUIDAsync(facilityId);

            return PagedList<DoctorDto>.ToPagedList(
                facility.Doctors.Select(d => _mapper.Map<DoctorDto>(d)).ToList(),
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<PagedList<DoctorDto>> AddDoctor(PagingParameters pagingParameters, Guid facilityId, Guid doctorId)
        {
            var facility = await _facilityCRUD.Reader.GetFacilityWithDoctorsByUUIDAsync(facilityId);

            if (facility.Doctors.FirstOrDefault(d => d.UUID == doctorId) is not null)
                throw new AlreadyExistsException($"Doctor is already added to this facility.", $"DoctorId = {doctorId}, FacilityId = {facilityId}");

            var doctor = await _doctorReader.GetByUUIDAsync(doctorId);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    facility.Doctors.Add(doctor);

                    await _facilityCRUD.Updater.UpdateAsync(facility, LoggedProfile);
                    await _facilityCRUD.Creator.CreateEventAsync(facility, FacilityEventType.Type.Updated, LoggedProfile, $"Added doctor: {doctor.ToString()}");

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return PagedList<DoctorDto>.ToPagedList(
                facility.Doctors.Select(d => _mapper.Map<DoctorDto>(d)).ToList(),
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<PagedList<DoctorDto>> RemoveDoctor(PagingParameters pagingParameters, Guid facilityId, Guid doctorId)
        {
            var facility = await _facilityCRUD.Reader.GetFacilityWithDoctorsByUUIDAsync(facilityId);
            var doctor = facility.Doctors.FirstOrDefault(d => d.UUID == doctorId);

            if (doctor is null)
                throw new AlreadyExistsException($"Doctor doesn't exists in this facility.", $"DoctorId = {doctorId}, FacilityId = {facilityId}");

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    facility.Doctors.Remove(doctor);

                    await _facilityCRUD.Updater.UpdateAsync(facility, LoggedProfile);
                    await _facilityCRUD.Creator.CreateEventAsync(facility, FacilityEventType.Type.Updated, LoggedProfile, $"Removed doctor: {doctor.ToString()}");

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return PagedList<DoctorDto>.ToPagedList(
                facility.Doctors.Select(d => _mapper.Map<DoctorDto>(d)).ToList(),
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<FacilityDto> Create(CreateFacilityDto request)
        {
            await _facilityChecker.CheckTINWithVATRegisterAsync(request.TaxIdentificationNumber);

            var facility = _mapper.Map<Facility>(request);
            await _facilityChecker.CheckFacilityAlreadyExistsAsync(facility);

            facility.UUID = Guid.NewGuid();
            facility.StatusId = FacilityStatus.Status.Prepared;

            //TODO:
            facility.ProfileId = 4;

            var address = _mapper.Map<Address>(request.Address);
            address.Country = await _facilityCRUD.Reader.GetCountryByIdAsync(request.Address.CountryId);
            await _facilityChecker.CheckAddressAlreadyExistsAsync(address);

            address.UUID = Guid.NewGuid();
            address.StatusId = AddressStatus.Status.Active;

            facility.Addresses.Add(address);
            
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await _facilityCRUD.Creator.CreateAsync(facility, LoggedProfile);
                    await _facilityCRUD.Creator.CreateEventAsync(facility, FacilityEventType.Type.Created, LoggedProfile, facility.ToString());

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return _mapper.Map<FacilityDto>(facility);
        }

        public async Task<FacilityDto> Update(UpdateFacilityDto request)
        {
            var facility = await _facilityCRUD.Reader.GetByUUIDAsync(request.Id);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    facility.ShortName = request.ShortName;
                    facility.Name = request.Name;

                    await _facilityCRUD.Updater.UpdateAsync(facility, LoggedProfile);
                    await _facilityCRUD.Creator.CreateEventAsync(facility, FacilityEventType.Type.Updated, LoggedProfile, facility.ToString());

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return _mapper.Map<FacilityDto>(facility);
        }

        public async Task Delete(Guid id)
        {
            var facility = await _facilityCRUD.Reader.GetByUUIDAsync(id);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await _facilityCRUD.Deleter.DeleteAsync(facility, LoggedProfile);
                    await _facilityCRUD.Creator.CreateEventAsync(facility, FacilityEventType.Type.Deleted, LoggedProfile, facility.ToString());

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };
        }

        public async Task<FacilityDto> CreateAddress(Guid facilityId, CreateAddressDto request)
        {
            var facility = await _facilityCRUD.Reader.GetByUUIDAsync(facilityId);

            var address = _mapper.Map<Address>(request);
            address.Country = await _facilityCRUD.Reader.GetCountryByIdAsync(request.CountryId);
            await _facilityChecker.CheckAddressAlreadyExistsAsync(address);

            address.UUID = Guid.NewGuid();
            address.StatusId = AddressStatus.Status.Active;
            
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    facility.Addresses.Add(address);

                    await _facilityCRUD.Updater.UpdateAsync(facility, LoggedProfile);
                    await _facilityCRUD.Creator.CreateEventAsync(facility, FacilityEventType.Type.Updated, LoggedProfile, $"Added new address: {address.ToString()}");

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return _mapper.Map<FacilityDto>(facility);
        }

        public async Task<FacilityDto> UpdateAddress(Guid facilityId, UpdateAddressDto request)
        {
            var facility = await _facilityCRUD.Reader.GetByUUIDAsync(facilityId);
            var address = facility.Addresses.Find(a => a.UUID == request.Id);
            if (address is null)
                throw new NotFoundException("Address not found.");

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    address.Name = request.Name;
                    address.StreetTypeId = (AddressStreetType.StreetType)request.StreetTypeId;
                    address.StreetName = request.StreetName;
                    address.HouseNumber = request.HouseNumber;
                    address.FlatNumber = request.FlatNumber;
                    address.City = request.City;
                    address.PostalCode = request.PostalCode;
                    address.Country = await _facilityCRUD.Reader.GetCountryByIdAsync(request.CountryId);
                    await _facilityChecker.CheckAddressAlreadyExistsAsync(address);

                    await _facilityCRUD.Updater.UpdateAsync(facility, LoggedProfile);
                    await _facilityCRUD.Creator.CreateEventAsync(facility, FacilityEventType.Type.Updated, LoggedProfile, $"Updated address: {address.ToString()}");

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return _mapper.Map<FacilityDto>(facility);
        }

        public async Task<FacilityDto> DeleteAddress(Guid facilityId, Guid addressId)
        {
            var facility = await _facilityCRUD.Reader.GetByUUIDAsync(facilityId);
            var address = facility.Addresses.Find(a => a.UUID == addressId);
            if (address is null)
                throw new NotFoundException("Address not found.");

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    address.StatusId = AddressStatus.Status.Deleted;

                    await _facilityCRUD.Updater.UpdateAsync(facility, LoggedProfile);
                    await _facilityCRUD.Creator.CreateEventAsync(facility, FacilityEventType.Type.Updated, LoggedProfile, $"Deleted address: {addressId}");

                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return _mapper.Map<FacilityDto>(await _facilityCRUD.Reader.GetByUUIDAsync(facilityId));
        }
 
    }
}