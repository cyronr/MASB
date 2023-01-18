using MABS.Application.CRUD.Creators.DoctorCreators;
using MABS.Application.CRUD.Deleters.DoctorDeleters;
using MABS.Application.CRUD.Readers.DoctorReaders;
using MABS.Application.CRUD.Updaters.DoctorUpdaters;

namespace MABS.Application.CRUD
{
    public interface IDoctorCRUD
    {
        public IDoctorCreator Creator { get; }
        public IDoctorReader Reader { get; }
        public IDoctorUpdater Updater { get; }
        public IDoctorDeleter Deleter { get; }
    }

    public class DoctorCRUD : IDoctorCRUD
    {
        public IDoctorCreator Creator { get; }
        public IDoctorReader Reader { get; }
        public IDoctorUpdater Updater { get; }
        public IDoctorDeleter Deleter { get; }

        public DoctorCRUD(
            IDoctorCreator creator,
            IDoctorReader reader,
            IDoctorUpdater updater,
            IDoctorDeleter deleter
            )
        {
            this.Creator = creator;
            this.Reader = reader;
            this.Updater = updater;
            this.Deleter = deleter;
        }
    }
}
