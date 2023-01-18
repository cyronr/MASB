using MABS.Application.CRUD.Creators.PatientCreators;
using MABS.Application.CRUD.Deleters.PatientDeleters;
using MABS.Application.CRUD.Readers.PatientReaders;
using MABS.Application.CRUD.Updaters.PatientUpdaters;

namespace MABS.Application.CRUD
{
    public interface IPatientCRUD
    {
        public IPatientCreator Creator { get; }
        public IPatientReader Reader { get; }
        public IPatientUpdater Updater { get; }
        public IPatientDeleter Deleter { get; }
    }

    public class PatientCRUD : IPatientCRUD
    {
        public IPatientCreator Creator { get; }
        public IPatientReader Reader { get; }
        public IPatientUpdater Updater { get; }
        public IPatientDeleter Deleter { get; }

        public PatientCRUD(
            IPatientCreator creator,
            IPatientReader reader,
            IPatientUpdater updater,
            IPatientDeleter deleter
            )
        {
            this.Creator = creator;
            this.Reader = reader;
            this.Updater = updater;
            this.Deleter = deleter;
        }
    }
}
