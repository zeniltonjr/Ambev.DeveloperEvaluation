namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdatePuduct
{
    public class UpdateProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public Guid BranchId { get; set; }
        public string Message { get; set; }
    }
}
