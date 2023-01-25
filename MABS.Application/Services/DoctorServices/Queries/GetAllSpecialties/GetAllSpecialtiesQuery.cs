using MABS.Application.Services.DoctorServices.Common;
using MediatR;

namespace MABS.Application.Services.DoctorServices.Queries.GetAllSpecialties
{
    public record GetAllSpecialtiesQuery() : IRequest<List<SpecialityExtendedDto>>;
}
