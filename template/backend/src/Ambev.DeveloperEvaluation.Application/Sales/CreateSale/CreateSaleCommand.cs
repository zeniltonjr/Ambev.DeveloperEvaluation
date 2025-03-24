using Ambev.DeveloperEvaluation.Application.SaleItens.CreateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public string SaleNumber { get; set; }

        public DateTime SaleDate { get; set; }

        public Guid CustomerId { get; set; }

        public Guid BranchId { get; set; }

        public bool IsCancelled { get; set; }

        public decimal TotalAmount { get; set; }

        public List<CreateSaleItemCommand> Items { get; set; }
    }
}
