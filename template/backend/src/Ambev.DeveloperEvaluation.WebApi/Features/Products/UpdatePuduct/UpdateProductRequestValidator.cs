using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdatePudct
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

            RuleFor(x => x.BasePrice)
                .GreaterThan(0).WithMessage("Base price must be greater than zero.");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");
        }
    }
}
