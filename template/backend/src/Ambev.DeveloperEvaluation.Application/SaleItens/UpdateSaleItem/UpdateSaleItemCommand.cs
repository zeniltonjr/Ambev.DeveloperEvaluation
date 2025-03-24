using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItens.UpdateSaleItem
{
    public class UpdateSaleItemCommand : IRequest<UpdateSaleItemResult>
    {
        public Guid SaleId { get; set; } 

        public Guid SaleItemId { get; set; }  

        public Guid ProductId { get; set; } 

        public int Quantity { get; set; }  

        public decimal UnitPrice { get; set; }   
    }
}
