using MABS.Application.Common.AppProfile;

namespace MABS.Application.CRUD.Updaters
{
    public interface IUpdater<T>
    {
        Task UpdateAsync(T entity, CallerProfile callerProfile);
    }
}
