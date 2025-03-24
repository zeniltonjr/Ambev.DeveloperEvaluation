using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public class GetSaleRequestValidator : AbstractValidator<GetSalesQueryRequest>
    {
        public GetSaleRequestValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty()
               .WithMessage("Sale ID is required");
        }
    }
}
