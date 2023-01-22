using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.AllTitles
{
    public record AllTitlesQuery() : IRequest<List<TitleExtendedDto>>;
}
