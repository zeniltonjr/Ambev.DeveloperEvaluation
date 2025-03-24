using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        public UpdateSaleRequestValidator()
        {
            RuleFor(x => x.SaleNumber)
               .NotEmpty().WithMessage("Sale number is required.");
            RuleFor(x => x.SaleDate)
                .NotEmpty().WithMessage("Sale date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Sale date cannot be in the future.");
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");
            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");
            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than 0.");
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one sale item is required.")
                .Must(items => items.All(i => i.Quantity > 0 && i.Price > 0))
                .WithMessage("Each sale item must have a quantity greater than 0 and a price greater than 0.");
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty().WithMessage("Product ID is required.");
                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
                item.RuleFor(i => i.Price)
                    .GreaterThan(0).WithMessage("Price must be greater than 0.");
            });
        }
    }
}
