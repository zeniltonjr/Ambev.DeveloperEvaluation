using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.CreateSaleItem
{
    public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
    {
        public CreateSaleItemRequestValidator()
        {
            RuleFor(x => x.SaleId)
           .NotEmpty().WithMessage("SaleId must be provided.");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId must be provided.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
                .LessThanOrEqualTo(20).WithMessage("Quantity cannot be greater than 20.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("UnitPrice must be greater than 0.");
        }
    }
}
