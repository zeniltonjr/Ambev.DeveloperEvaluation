using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        public string SaleNumber { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; }

        public Guid CustomerId { get; set; }

        public Guid BranchId { get; set; }

        public decimal TotalAmount { get; set; }

        public bool IsCancelled { get; set; }

        public ICollection<CreateSaleItemSaleRequest> Items { get; set; } = new List<CreateSaleItemSaleRequest>();
    }
}
