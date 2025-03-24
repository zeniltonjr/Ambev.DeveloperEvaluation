namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdatePudct
{
    public class UpdateProductRequest
    {
        public string Name { get; set; } = string.Empty;

        public decimal BasePrice { get; set; }

        public Guid BranchId { get; set; }

    }
}
