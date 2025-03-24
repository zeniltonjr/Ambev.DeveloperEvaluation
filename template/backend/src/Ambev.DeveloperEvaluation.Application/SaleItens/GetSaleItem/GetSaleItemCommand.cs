using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItens.GetSaleItem
{
    public record GetSaleItemCommand : IRequest<GetSaleItemResult>
    {
        public Guid Id { get; }

        public GetSaleItemCommand(Guid id)
        {
            Id = id;
        }
    }
}
