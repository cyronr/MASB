using MABS.Application.CRUD.Creators.FacilityCreators;
using MABS.Application.CRUD.Deleters.FacilityDeleters;
using MABS.Application.CRUD.Readers.FacilityReaders;
using MABS.Application.CRUD.Updaters.FacilityUpdaters;

namespace MABS.Application.CRUD
{
    public interface IFacilityCRUD
    {
        public IFacilityCreator Creator { get; }
        public IFacilityReader Reader { get; }
        public IFacilityUpdater Updater { get; }
        public IFacilityDeleter Deleter { get; }
    }

    public class FacilityCRUD : IFacilityCRUD
    {
        public IFacilityCreator Creator { get; }
        public IFacilityReader Reader { get; }
        public IFacilityUpdater Updater { get; }
        public IFacilityDeleter Deleter { get; }

        public FacilityCRUD(
            IFacilityCreator creator,
            IFacilityReader reader,
            IFacilityUpdater updater,
            IFacilityDeleter deleter
            )
        {
            this.Creator = creator;
            this.Reader = reader;
            this.Updater = updater;
            this.Deleter = deleter;
        }
    }
}
