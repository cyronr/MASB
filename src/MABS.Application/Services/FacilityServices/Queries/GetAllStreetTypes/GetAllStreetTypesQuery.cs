using MABS.Application.Services.FacilityServices.Common;
using MediatR;

namespace MABS.Application.Services.FacilityServices.Queries.GetAllStreetTypes
{
    public record GetAllStreetTypesQuery() : IRequest<List<StreetTypeExtendedDto>>;
}
