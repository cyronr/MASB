using MABS.Application.Common.AppProfile;

namespace MABS.Application.CRUD.Deleters
{
    public interface IDeleter<T>
    {
        Task DeleteAsync(T entity, CallerProfile callerProfile);
    }
}
