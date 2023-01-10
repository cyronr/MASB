using MABS.Domain.Models.DoctorModels;
using MABS.Domain.Models.FacilityModels;
using Microsoft.EntityFrameworkCore;
using MABS.Application.Repositories;
using Microsoft.Extensions.Logging;
using MABS.Domain.Models.DictionaryModels;

namespace MABS.Infrastructure.Data.Repositories.FacilityRepositories
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly ILogger<FacilityRepository> _logger;
        private readonly DataContext _context;

        public FacilityRepository(ILogger<FacilityRepository> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void Create(Facility facility)
        {
            _logger.LogInformation("Saving facility to databse.");
            _context.Facilites.Add(facility);
        }

        public void CreateEvent(FacilityEvent facilityEvent)
        {
            _logger.LogInformation($"Saving to database event {facilityEvent.TypeId} for doctor with id = {facilityEvent.Facility.Id}.");
            _context.FacilityEvents.Add(facilityEvent);
        }

        public async Task<List<Facility>> GetAll()
        {
            return await _context.Facilites
                .Include(f => f.Status)
                .Include(f => f.Events)
                    .ThenInclude(e => e.Type)
                .Include(f => f.Addresses)
                    .ThenInclude(a => a.Status)
                .Include(f => f.Addresses)
                    .ThenInclude(a => a.StreetType)
                .Include(f => f.Addresses)
                    .ThenInclude(a => a.Country)
                .Include(f => f.Addresses.Where(a => a.StatusId == AddressStatus.Status.Active))
                .Where(f => f.StatusId != FacilityStatus.Status.Deleted)
                .ToListAsync();
        }
        public async Task<Facility> GetByUUID(Guid uuid)
        {
            return await _context.Facilites
                .Include(f => f.Status)
                .Include(f => f.Events)
                    .ThenInclude(e => e.Type)
                .Include(f => f.Addresses)
                    .ThenInclude(a => a.Status)
                .Include(f => f.Addresses)
                    .ThenInclude(a => a.StreetType)
                .Include(f => f.Addresses.Where(a => a.StatusId == AddressStatus.Status.Active))
                .FirstOrDefaultAsync(f => f.StatusId != FacilityStatus.Status.Deleted && f.UUID == uuid);
        }

        public async Task<Facility> GetByTIN(string taxIdentificationNumber)
        {
            return await _context.Facilites
                .FirstOrDefaultAsync(f => f.StatusId != FacilityStatus.Status.Deleted && f.TaxIdentificationNumber == taxIdentificationNumber);
        }

        public async Task<Facility> GetWithAllDoctorsByUUID(Guid uuid)
        {
            return await _context.Facilites
                .Include(f => f.Doctors.Where(d => d.StatusId == DoctorStatus.Status.Active))
                    .ThenInclude(d => d.Title)
                .Include(f => f.Doctors)
                    .ThenInclude(d => d.Specialties)
                .FirstOrDefaultAsync(f => f.StatusId != FacilityStatus.Status.Deleted && f.UUID == uuid);
        }

        public async Task<Country> GetCountryById(string id)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Address> GetAddressByProperties(string streetName, int houseNumber, int? flatNumber, string city, string postalCode, Country country)
        {
            return await _context.Addresses
                .Include(a => a.Country)
                .FirstOrDefaultAsync(
                    a =>
                        a.StatusId == AddressStatus.Status.Active &&
                        a.StreetName == streetName &&
                        a.HouseNumber == houseNumber &&
                        a.FlatNumber == flatNumber &&
                        a.City == city &&
                        a.PostalCode == postalCode &&
                        a.Country.Equals(country)
                );
        }

        public async Task<List<AddressStreetType>> GetAllStreetTypes()
        {
            return await _context.AddressStreetType.ToListAsync();
        }
    }
}