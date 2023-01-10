using AutoMapper;
using MABS.Application.Common.Pagination;
using MABS.Application.DTOs.DoctorDtos;
using MABS.Application.DTOs.FacilityDtos;
using MABS.Domain.Models.FacilityModels;
using MABS.Application.Services.Helpers.DoctorHelpers;
using MABS.Application.Services.Helpers.FacilityHelpers;
using System.Net;
using Microsoft.Extensions.Logging;
using MABS.Application.DataAccess.Repositories;
using MABS.Application.DataAccess.Common;
using MABS.Domain.Exceptions;

namespace MABS.Application.Services.FacilityServices
{
    public class FacilityService : IFacilityService
    {
        private readonly ILogger<FacilityService> _logger;
        private readonly IFacilityRepository _facilityRepository;
        private readonly IMapper _mapper;
        private readonly IDbOperation _db;
        private readonly IFacilityHelper _facilityHelper;
        private readonly IDoctorHelper _doctorHelper;

        public FacilityService(ILogger<FacilityService> logger, IMapper mapper, IDbOperation dbOperation, IFacilityRepository facilityRepository, IFacilityHelper facilityHelper, IDoctorHelper doctorHelper)
        {
            _db = dbOperation;
            _mapper = mapper;
            _logger = logger;
            _facilityRepository = facilityRepository;
            _facilityHelper = facilityHelper;
            _doctorHelper = doctorHelper;
        }

        public async Task<PagedList<FacilityDto>> GetAll(PagingParameters pagingParameters)
        {
            var facilities = await _facilityRepository.GetAll();
            return PagedList<FacilityDto>.ToPagedList(
                facilities.Select(f => _mapper.Map<FacilityDto>(f)).ToList(),
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<FacilityDto> GetById(Guid id)
        {
            var facility = await _facilityHelper.GetFacilityByUUID(id);
            return _mapper.Map<FacilityDto>(facility);
        }

        public async Task<List<CountryDto>> GetAllCountries()
        {
            var countries = await _facilityRepository.GetAllCountries();
            return countries.Select(c => _mapper.Map<CountryDto>(c)).ToList();
        }

        public async Task<List<StreetTypeExtendedDto>> GetAllStreetTypes()
        {
            var streetTypes = await _facilityRepository.GetAllStreetTypes();
            return streetTypes.Select(c => _mapper.Map<StreetTypeExtendedDto>(c)).ToList();
        }
        public async Task<PagedList<DoctorDto>> GetAllDoctors(PagingParameters pagingParameters, Guid facilityId)
        {
            var facility = await _facilityHelper.GetFacilityWithDoctorsByUUID(facilityId);

            return PagedList<DoctorDto>.ToPagedList(
                facility.Doctors.Select(d => _mapper.Map<DoctorDto>(d)).ToList(),
                pagingParameters.PageNumber,
                pagingParameters.PageSize
            );
        }

        public async Task<PagedList<DoctorDto>> AddDoctor(PagingParameters pagingParameters, Guid facilityId, Guid doctorId)
        {
            var facility = await _facilityHelper.GetFacilityWithDoctorsByUUID(facilityId);

            if (facility.Doctors.FirstOrDefault(d => d.UUID == doctorId) != null)
                throw new AlreadyExistsException($"Doctor is already added to this facility.", $"DoctorId = {doctorId}, FacilityId = {facilityId}");

            var doctor = await _doctorHelper.GetDoctorByUUID(doctorId);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    facility.Doctors.Add(doctor);
                    await DoUpdateFacility(facility, $"Added doctor: {doctor.ToString()}");

                    await _db.Save();
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
            var facility = await _facilityHelper.GetFacilityWithDoctorsByUUID(facilityId);
            var doctor = facility.Doctors.FirstOrDefault(d => d.UUID == doctorId);

            if (doctor == null)
                throw new AlreadyExistsException($"Doctor doesn't exists in this facility.", $"DoctorId = {doctorId}, FacilityId = {facilityId}");

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    facility.Doctors.Remove(doctor);
                    await DoUpdateFacility(facility, $"Removed doctor: {doctor.ToString()}");

                    await _db.Save();
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
            CheckTINWithVATRegister(request.TaxIdentificationNumber);

            var facility = _mapper.Map<Facility>(request);
            await _facilityHelper.CheckFacilityAlreadyExists(facility);

            facility.UUID = Guid.NewGuid();
            facility.StatusId = FacilityStatus.Status.Prepared;

            var address = _mapper.Map<Address>(request.Address);
            address.Country = await _facilityHelper.GetCountryById(request.Address.CountryId);
            await _facilityHelper.CheckAddressAlreadyExists(address);

            address.UUID = Guid.NewGuid();
            address.StatusId = AddressStatus.Status.Active;

            facility.Addresses.Add(address);
            
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await DoCreateFacility(facility);
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
            var facility = await _facilityHelper.GetFacilityByUUID(request.Id);//_mapper.Map<Facility>(request);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    facility.ShortName = request.ShortName;
                    facility.Name = request.Name;
                    await DoUpdateFacility(facility);

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
            var facility = await _facilityHelper.GetFacilityByUUID(id);
            facility.StatusId = FacilityStatus.Status.Deleted;

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await DoDeleteFacility(facility);
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
            var facility = await _facilityHelper.GetFacilityByUUID(facilityId);

            var address = _mapper.Map<Address>(request);
            address.Country = await _facilityHelper.GetCountryById(request.CountryId);
            await _facilityHelper.CheckAddressAlreadyExists(address);

            address.UUID = Guid.NewGuid();
            address.StatusId = AddressStatus.Status.Active;
            facility.Addresses.Add(address);
            
            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await DoUpdateFacility(facility, $"Added new address: {address.ToString()}");
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
            var facility = await _facilityHelper.GetFacilityByUUID(facilityId);
            var address = facility.Addresses.Find(a => a.UUID == request.Id);
            if (address == null)
                throw new NotFoundException("Address not found.");

            //address = _mapper.Map<Address>(request);
            address.Name = request.Name;
            address.StreetTypeId = (AddressStreetType.StreetType)request.StreetTypeId;
            address.StreetName = request.StreetName;
            address.HouseNumber = request.HouseNumber;
            address.FlatNumber = request.FlatNumber;
            address.City = request.City;
            address.PostalCode = request.PostalCode;
            address.Country = await _facilityHelper.GetCountryById(request.CountryId);
            await _facilityHelper.CheckAddressAlreadyExists(address);

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await DoUpdateFacility(facility, $"Updated address: {address.ToString()}");
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
            var facility = await _facilityHelper.GetFacilityByUUID(facilityId);
            var address = facility.Addresses.Find(a => a.UUID == addressId);
            if (address == null)
                throw new NotFoundException("Address not found.");

            address.StatusId = AddressStatus.Status.Deleted;

            using (var tran = _db.BeginTransaction())
            {
                try
                {
                    await DoUpdateFacility(facility, $"Deleted address: {addressId}");
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            };

            return _mapper.Map<FacilityDto>(await _facilityHelper.GetFacilityByUUID(facilityId));
        }

        private async Task DoCreateFacility(Facility facility)
        {
            _facilityRepository.Create(facility);
            await _db.Save();

            _facilityRepository.CreateEvent(new FacilityEvent
            {
                TypeId = FacilityEventType.Type.Created,
                Facility = facility,
                AddInfo = facility.ToString()
            });
            await _db.Save();
        }

        private async Task DoUpdateFacility(Facility facility)
        {
            _facilityRepository.CreateEvent(new FacilityEvent
            {
                TypeId = FacilityEventType.Type.Updated,
                Facility = facility,
                AddInfo = facility.ToString()
            });
            await _db.Save();
        }

        private async Task DoUpdateFacility(Facility facility, string addInfo)
        {
            _facilityRepository.CreateEvent(new FacilityEvent
            {
                TypeId = FacilityEventType.Type.Updated,
                Facility = facility,
                AddInfo = addInfo
            });
            await _db.Save();
        }

        private async Task DoDeleteFacility(Facility facility)
        {
            _facilityRepository.CreateEvent(new FacilityEvent
            {
                TypeId = FacilityEventType.Type.Deleted,
                Facility = facility
            });
            await _db.Save();
        }

        public void CheckTINWithVATRegister(string taxIdentificationNumber)
        {
            _logger.LogInformation($"Checking facility's TIN with VAT Register.");

            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string url = $@"https://wl-api.mf.gov.pl//api/search/nip/{taxIdentificationNumber}?date={date}";

            try
            {
                _logger.LogDebug($"Sending request {url}.");
                _logger.LogInformation($"Sending request {url}.");

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    LogHTTPResponse(response);
                }
            }
            catch (WebException ex)
            {
                _logger.LogInformation(ex.Message);
                LogHTTPResponse((HttpWebResponse)ex.Response);
                throw new WrongTaxIdentificationNumberException($"{taxIdentificationNumber} was not found in VAT Register.");
            }
        }

        private void LogHTTPResponse(HttpWebResponse response)
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
                return;

            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var body = reader.ReadToEnd();
                _logger.LogDebug($"Request returned with response status {response.StatusCode} and body {body}");
            }
        }
    }
}