using Ambev.DeveloperEvaluation.Application.SaleItens.UpdateSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleItemCommand : IRequest<UpdateSaleResult>
    {
        public Guid SaleId { get; }
        public string SaleNumber { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<SaleItens.UpdateSaleItem.UpdateSaleItemCommand> Items { get; set; }
    }
}
