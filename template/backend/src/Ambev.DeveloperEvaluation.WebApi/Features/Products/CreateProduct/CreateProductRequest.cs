namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public Guid BranchId { get; set; }
    }
}
