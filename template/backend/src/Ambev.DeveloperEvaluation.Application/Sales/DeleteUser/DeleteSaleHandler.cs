using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteUser
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResponse>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ILogger<DeleteSaleHandler> _logger;

        public DeleteSaleHandler(ISaleRepository saleRepository, ILogger<DeleteSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DeleteSaleResponse> Handle(DeleteSaleCommand request,
                                                     CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteSaleCommand for Sale with ID {SaleId}", request.Id);

            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
            {
                _logger.LogError("Sale with ID {SaleId} not found.", request.Id);
                throw new NotFoundException($"Sale with ID {request.Id} not found.");
            }

            await _saleRepository.DeleteAsync(sale.Id, cancellationToken);

            _logger.LogInformation("Sale with ID {SaleId} deleted successfully.", request.Id);

            return new DeleteSaleResponse(true, $"Sale with ID {request.Id} deleted successfully.");
        }
    }
}
