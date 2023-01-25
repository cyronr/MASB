using MABS.API.Requests.FacilityRequests;

namespace MABS.API.Validators.FacilityValidators
{
    public class CreateFacilityRequestValidator : UpsertFacilityRequestValidator<CreateFacilityRequest>
    {
        public CreateFacilityRequestValidator()
        {
            RuleFor(obj => obj.Address).SetValidator(new CreateAddressRequestValidator());
        }
    }
}