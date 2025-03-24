using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs.GetBranch
{
    public class GetBranchResponse
    {
        public Guid Id { get; set; } 
     
        public string Name { get; set; } = string.Empty;

        public List<GetProductResponse> Products { get; set; } = new List<GetProductResponse>();
    }
}
