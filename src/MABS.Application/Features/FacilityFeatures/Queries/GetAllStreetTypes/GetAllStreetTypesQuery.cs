using MABS.Application.Features.FacilityFeatures.Common;
using MediatR;

namespace MABS.Application.Features.FacilityFeatures.Queries.GetAllStreetTypes
{
    public record GetAllStreetTypesQuery() : IRequest<List<StreetTypeExtendedDto>>;
}
