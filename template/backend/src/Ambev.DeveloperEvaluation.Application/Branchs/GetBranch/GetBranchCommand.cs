using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public class GetBranchCommand : IRequest<GetBranchResult>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Initializes a new instance of GetUserCommand
        /// </summary>
        /// <param name="id">The ID of the user to retrieve</param>
        public GetBranchCommand(Guid id)
        {
            Id = id;
        }
    }
}
