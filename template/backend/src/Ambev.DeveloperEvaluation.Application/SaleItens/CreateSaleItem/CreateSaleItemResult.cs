namespace Ambev.DeveloperEvaluation.Application.SaleItens.CreateSaleItem
{
    public class CreateSaleItemResult
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
