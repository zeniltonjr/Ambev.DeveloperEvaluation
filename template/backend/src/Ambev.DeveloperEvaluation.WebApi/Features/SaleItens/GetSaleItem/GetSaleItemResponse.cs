using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.GetSaleItem
{
    public class GetSaleItemResponse
    {
        public Guid SaleId { get; private set; }

        public Guid ProductId { get; private set; }

        public int Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal Discount { get; private set; }

        public decimal TotalPrice => (UnitPrice * Quantity) - Discount;
    }
}
