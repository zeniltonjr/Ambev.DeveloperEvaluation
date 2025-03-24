using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.GetSaleItem
{
    public class GetSaleItemRequestValidator : AbstractValidator<GetSaleItemRequest>
    {
        public GetSaleItemRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
