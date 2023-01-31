using System.Data;
using MABS.Application.DataAccess.Common;
using MABS.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace MABS.Infrastructure.DataAccess.Common
{
    public class DbOperation : IDbOperation
    {
        private readonly ILogger<DbOperation> _logger;
        private readonly DataContext _context;

        public DbOperation(ILogger<DbOperation> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IInternalDbTransaction BeginTransaction()
        {
            _logger.LogInformation("Begining new database transaction.");
            return new InternalDbTransaction(_context);
        }

        public IInternalDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            _logger.LogInformation($"Begining new database transaction with isolation level {isolationLevel}.");
            return new InternalDbTransaction(_context, isolationLevel);
        }

        public bool IsActiveTransaction()
        {
            if (_context.Database.CurrentTransaction is null)
                return false;

            return true;
        }

        public async Task<int> Save()
        {
            _logger.LogInformation("Saving changes to database.");
            return await _context.SaveChangesAsync();
        }
    }
}