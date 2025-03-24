using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetAllSalesQuery : IRequest<List<GetSaleResult>>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
        public string SortBy { get; set; } = "saleDate"; 
        public bool SortOrder { get; set; } = true;      
    }
}
