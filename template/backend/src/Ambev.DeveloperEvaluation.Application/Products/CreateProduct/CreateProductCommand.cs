using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductCommand : IRequest<CreateProductResult>
    {
        public string Name { get; set; } = string.Empty;

        public decimal BasePrice { get; set; }

        public Guid BranchId { get; set; }
    }
}
