namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleItemSaleRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
