using MABS.Application.DataAccess.Common;
using MABS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace MABS.Infrastructure.DataAccess.Common
{
    public class InternalDbTransaction : IInternalDbTransaction
    {
        private IDbContextTransaction _transaction;

        public InternalDbTransaction(DataContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        public InternalDbTransaction(DataContext context, System.Data.IsolationLevel isolationLevel)
        {
            _transaction = context.Database.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            _transaction.CommitAsync();
        }

        public void Dispose()
        {
            _transaction.DisposeAsync();
        }

        public void Rollback()
        {
            _transaction.RollbackAsync();
        }
    }
}