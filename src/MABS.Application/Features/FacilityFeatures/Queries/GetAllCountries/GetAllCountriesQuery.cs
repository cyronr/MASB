using MABS.Application.Features.FacilityFeatures.Common;
using MediatR;

namespace MABS.Application.Features.FacilityFeatures.Queries.GetAllCountries
{
    public record GetAllCountriesQuery() : IRequest<List<CountryDto>>;
}
