using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public class GetAllBranchesQuery : IRequest<List<GetBranchResult>>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}
