namespace MABS.Application.Repositories
{
    public interface IInternalDbTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}