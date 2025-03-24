using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItens.GetSaleItem
{
    public class GetAllSaleItemsQuery : IRequest<List<GetSaleItemResult>>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}
