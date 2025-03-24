using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct
{
    public class GetProductResponse
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public Guid BranchId { get; set; }
    }
}
