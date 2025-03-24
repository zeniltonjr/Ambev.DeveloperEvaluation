namespace Ambev.DeveloperEvaluation.Application.SaleItens.UpdateSaleItem
{
    public class UpdateSaleItemResult
    {
        public Guid SaleItemId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalPrice { get; set; }

        public string Message { get; set; }
    }
}
