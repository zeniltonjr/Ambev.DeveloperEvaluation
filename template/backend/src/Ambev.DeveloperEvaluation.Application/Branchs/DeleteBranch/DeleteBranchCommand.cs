using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.DeleteBranch
{
    public record DeleteBranchCommand : IRequest<Unit>
    {
        public Guid Id { get; }

        public DeleteBranchCommand(Guid id)
        {
            Id = id;
        }
    }
}
