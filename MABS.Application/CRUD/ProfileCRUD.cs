using MABS.Application.CRUD.Creators.ProfileCreators;
using MABS.Application.CRUD.Deleters.ProfileDeleters;
using MABS.Application.CRUD.Readers.ProfileReaders;
using MABS.Application.CRUD.Updaters.ProfileUpdaters;

namespace MABS.Application.CRUD
{
    public interface IProfileCRUD
    {
        public IProfileCreator Creator { get; }
        public IProfileReader Reader { get; }
        public IProfileUpdater Updater { get; }
        public IProfileDeleter Deleter { get; }
    }

    public class ProfileCRUD : IProfileCRUD
    {
        public IProfileCreator Creator { get; }
        public IProfileReader Reader { get; }
        public IProfileUpdater Updater { get; }
        public IProfileDeleter Deleter { get; }

        public ProfileCRUD(
            IProfileCreator creator,
            IProfileReader reader,
            IProfileUpdater updater,
            IProfileDeleter deleter
            )
        {
            this.Creator = creator;
            this.Reader = reader;
            this.Updater = updater;
            this.Deleter = deleter;
        }
    }
}
