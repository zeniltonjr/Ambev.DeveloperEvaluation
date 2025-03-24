using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteUser
{
    public record DeleteSaleCommand : IRequest<DeleteSaleResponse>
    {
        /// <summary>
        /// The unique identifier of the user to delete
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Initializes a new instance of DeleteUserCommand
        /// </summary>
        /// <param name="id">The ID of the user to delete</param>
        public DeleteSaleCommand(Guid id)
        {
            Id = id;
        }
    }
}
