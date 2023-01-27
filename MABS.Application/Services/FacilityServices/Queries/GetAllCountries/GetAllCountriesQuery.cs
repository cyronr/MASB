using MABS.Application.Services.FacilityServices.Common;
using MediatR;

namespace MABS.Application.Services.FacilityServices.Queries.GetAllCountries
{
    public record GetAllCountriesQuery() : IRequest<List<CountryDto>>;
}
