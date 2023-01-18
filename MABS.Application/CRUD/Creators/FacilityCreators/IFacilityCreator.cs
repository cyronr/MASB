using MABS.Domain.Models.FacilityModels;

namespace MABS.Application.CRUD.Creators.FacilityCreators
{
    public interface IFacilityCreator : ICreator<Facility, FacilityEventType.Type>
    {
    }
}
