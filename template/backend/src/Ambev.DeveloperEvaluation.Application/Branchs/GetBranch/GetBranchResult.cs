using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public class GetBranchResult
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<GetProductResult> Products { get; set; } = new List<GetProductResult>();
    }
}
