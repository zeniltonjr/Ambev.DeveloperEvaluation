using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.DeleteSaleItem
{
    public class DeleteSaleItemRequestValidator : AbstractValidator<DeleteSaleItemRequest>
    {
        public DeleteSaleItemRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
