namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleItemRequest
    {
        public Guid ProductId { get; set; }  // Referência ao Produto

        public int Quantity { get; set; }    // Quantidade comprada

        public decimal UnitPrice { get; set; } // Preço unitário do produto

        public decimal Discount { get; set; }  // Desconto (se houver)
    }
}
