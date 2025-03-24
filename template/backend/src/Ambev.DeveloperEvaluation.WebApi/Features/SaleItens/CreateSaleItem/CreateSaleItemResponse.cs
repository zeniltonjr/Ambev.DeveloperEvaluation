namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.CreateSaleItem
{
    public class CreateSaleItemResponse
    {
        public Guid Id { get; set; }
        public Guid SaleId { get; internal set; }
        public Guid ProductId { get; internal set; }
        public int Quantity { get; internal set; }
        public decimal UnitPrice { get; internal set; }
        public decimal Discount { get; internal set; }
        public decimal TotalPrice { get; internal set; }
    }
}
