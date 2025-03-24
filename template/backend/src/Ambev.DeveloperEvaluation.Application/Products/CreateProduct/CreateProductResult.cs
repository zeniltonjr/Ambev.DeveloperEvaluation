namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductResult
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal BasePrice { get; set; }

        public Guid BranchId { get; set; }
    }
}
