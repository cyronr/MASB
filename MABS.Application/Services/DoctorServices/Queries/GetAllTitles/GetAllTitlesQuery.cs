using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.GetAllTitles
{
    public record AllTitlesQuery() : IRequest<List<TitleExtendedDto>>;
}
