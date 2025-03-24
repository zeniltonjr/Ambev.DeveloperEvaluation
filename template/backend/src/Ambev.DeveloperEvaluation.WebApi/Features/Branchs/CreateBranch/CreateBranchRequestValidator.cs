using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.CreateBranch
{
    public class CreateBranchRequestValidator : AbstractValidator<CreateBranchRequest>
    {
        public CreateBranchRequestValidator()
        {
            RuleFor(branch => branch.Name)
                .NotEmpty().WithMessage("O nome da filial é obrigatório.")
                .Length(3, 100).WithMessage("O nome da filial deve ter entre 3 e 100 caracteres.");
        }
    }
}
