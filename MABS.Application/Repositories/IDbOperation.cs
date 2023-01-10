using System.Data;

namespace MABS.Application.Repositories
{
    public interface IDbOperation
    {
        Task<int> Save();
        IInternalDbTransaction BeginTransaction();
        IInternalDbTransaction BeginTransaction(IsolationLevel isolationLevel);
        bool IsActiveTransaction();
    }
}