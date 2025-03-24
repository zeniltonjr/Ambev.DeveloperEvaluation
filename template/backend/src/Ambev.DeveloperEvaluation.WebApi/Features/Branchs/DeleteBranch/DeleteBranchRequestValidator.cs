using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.DeleteBranch
{
    public class DeleteBranchRequestValidator : AbstractValidator<DeleteBranchRequest>
    {
        public DeleteBranchRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}
