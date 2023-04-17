using MABS.Application.DataAccess.Repositories;
using MABS.Domain.Models.DictionaryModels;
using MABS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MABS.Infrastructure.DataAccess.Repositories
{
    public class DictionaryRepository : IDictionaryRepository
    {
        private readonly ILogger<DictionaryRepository> _logger;
        private readonly DataContext _context;
        public DictionaryRepository(ILogger<DictionaryRepository> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<City>> GetAllCitiesAsync()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<City?> GetCityByIdAsync(int id)
        {
            return await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
