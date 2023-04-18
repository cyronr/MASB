using MABS.Application.Features.DictionaryFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DictionaryFeatures.Queries.GetAllCities
{
    public record GetAllCitiesQuery() : IRequest<List<CityDto>>;
}
