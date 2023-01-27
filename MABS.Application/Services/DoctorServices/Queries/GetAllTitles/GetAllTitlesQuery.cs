using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.GetAllTitles
{
    public record GetAllTitlesQuery() : IRequest<List<TitleExtendedDto>>;
}
