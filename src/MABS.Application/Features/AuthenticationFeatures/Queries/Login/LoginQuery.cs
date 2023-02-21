using MABS.Application.Features.AuthenticationFeatures.Common;
using MediatR;

namespace MABS.Application.Features.AuthenticationFeatures.Queries.Login
{
    public record LoginQuery
    (
        string Email,
        string Password
    ) : IRequest<AuthenticationResultDto>;
}
