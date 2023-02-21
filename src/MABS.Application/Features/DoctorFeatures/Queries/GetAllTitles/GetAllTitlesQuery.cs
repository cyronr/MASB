using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Queries.GetAllTitles
{
    public record GetAllTitlesQuery() : IRequest<List<TitleExtendedDto>>;
}
