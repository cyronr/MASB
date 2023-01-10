using MABS.Application.Repositories;
using MABS.Domain.Models.ProfileModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MABS.Infrastructure.Data.CurrentLoggedProfile
{
    public class CurrentLoggedProfile : ICurrentLoggedProfile
    {
        private readonly DataContext _context;
        public CurrentLoggedProfile(ILogger<CurrentLoggedProfile> logger, DataContext context)
        {
            _context = context;
        }

        public Profile GetByUUID(Guid uuid)
        {
            return _context.Profiles
                .Include(p => p.Status)
                .Include(p => p.Type)
                .FirstOrDefault(p => p.StatusId != ProfileStatus.Status.Deleted && p.UUID == uuid);
        }
    }
}
