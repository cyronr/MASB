using FluentValidation;

namespace MABS.Application.Common.Pagination
{
    public class PagingParametersValidator : AbstractValidator<PagingParameters>
    {
        public PagingParametersValidator()
        {
            RuleFor(p => p.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater then 0.");

            RuleFor(p => p.PageSize)
                .GreaterThan(0)
                .WithMessage("Page number must be greater then 0.");
        }
    }
}
