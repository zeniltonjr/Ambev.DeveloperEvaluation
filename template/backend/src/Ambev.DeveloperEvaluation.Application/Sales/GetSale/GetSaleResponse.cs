using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleResponse
    {
        public Guid Id { get; set; }

        public string SaleNumber { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; }

        public Guid CustomerId { get; set; }

        public Guid BranchId { get; set; }

        public decimal TotalAmount { get; set; }

        public bool IsCancelled { get; set; }

        public ICollection<GetSaleItemSaleRequest> Items { get; set; } = new List<GetSaleItemSaleRequest>();
    }
}
