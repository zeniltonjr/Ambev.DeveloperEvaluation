using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public class GetSalesQueryValidator : AbstractValidator<GetSaleRequest>
    {
        public GetSalesQueryValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty()
               .WithMessage("User ID is required");
        }
    }
}
