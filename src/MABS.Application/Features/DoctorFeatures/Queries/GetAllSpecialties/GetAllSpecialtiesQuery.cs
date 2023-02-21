using MABS.Application.Features.DoctorFeatures.Common;
using MediatR;

namespace MABS.Application.Features.DoctorFeatures.Queries.GetAllSpecialties
{
    public record GetAllSpecialtiesQuery() : IRequest<List<SpecialityExtendedDto>>;
}
