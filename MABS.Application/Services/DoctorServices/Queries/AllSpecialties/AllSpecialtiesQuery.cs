using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.AllSpecialties
{
    public record AllSpecialtiesQuery() : IRequest<List<SpecialityExtendedDto>>;
}
