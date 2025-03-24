using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public record GetSaleCommand : IRequest<GetSaleResult>
    {
        public Guid Id { get; }

        /// <summary>
        /// Initializes a new instance of GetUserCommand
        /// </summary>
        /// <param name="id">The ID of the user to retrieve</param>
        public GetSaleCommand(Guid id)
        {
            Id = id;
        }
    }
}
