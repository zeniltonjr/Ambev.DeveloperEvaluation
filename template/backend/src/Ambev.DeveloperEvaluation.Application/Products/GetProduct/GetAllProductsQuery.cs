using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetAllProductsQuery : IRequest<List<GetProductResult>>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}
