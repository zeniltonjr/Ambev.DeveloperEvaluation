using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branchs.CreateBranch
{
    public class CreateBranchCommand : IRequest<CreateBranchResult>
    {
        public string Name { get;  set; }

        public List<Sale> Sales { get; set; } = new List<Sale>();
    }
}
