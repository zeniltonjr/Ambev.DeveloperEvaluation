namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleResponse
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; }

        public string BranchName { get; set; }

        public DateTime Date { get; set; }

        public List<SaleItemResponse> Items { get; set; }

    }

    public class SaleItemResponse
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
