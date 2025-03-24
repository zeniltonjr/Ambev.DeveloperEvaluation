using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItens.DeleteSaleItem
{
    public class DeleteSaleItemCommand : IRequest<DeleteSaleItemResponse>
    {
        public Guid Id { get; }

        public DeleteSaleItemCommand(Guid id)
        {
            Id = id;
        }
    }
}
