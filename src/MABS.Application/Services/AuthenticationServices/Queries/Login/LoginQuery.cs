using MABS.Application.Services.AuthenticationServices.Common;
using MediatR;

namespace MABS.Application.Services.AuthenticationServices.Queries.Login
{
    public record LoginQuery
    (
        string Email,
        string Password
    ) : IRequest<AuthenticationResultDto>;
}
