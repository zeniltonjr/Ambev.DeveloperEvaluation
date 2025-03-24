using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch
{
    public class UpdateBranchCommand : IRequest<UpdateBranchResult>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public UpdateBranchCommand() { }

        public UpdateBranchCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
