namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.UpdateSaleItem
{
    public class UpdateSaleItemRequest
    {
        public Guid saleId { get; set; }

        public Guid SaleItemId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
