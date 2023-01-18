using MABS.Application.Common.AppProfile;

namespace MABS.Application.CRUD.Creators
{
    public interface ICreator<Tentity, TentityEventType>
    {
        Task CreateAsync(Tentity entity, CallerProfile callerProfile);
        Task CreateEventAsync(Tentity entity, TentityEventType eventType, CallerProfile callerProfile, string addInfo);
    }
}
