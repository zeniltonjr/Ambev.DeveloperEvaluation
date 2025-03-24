using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.SaleItens.DeleteSaleItem
{
    public class DeleteSaleItemHandler : IRequestHandler<DeleteSaleItemCommand, DeleteSaleItemResponse>
    {
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly ILogger<DeleteSaleItemHandler> _logger;

        public DeleteSaleItemHandler(ISaleItemRepository saleItemRepository,
                                     ILogger<DeleteSaleItemHandler> logger)
        {
            _saleItemRepository = saleItemRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DeleteSaleItemResponse> Handle(DeleteSaleItemCommand request,
                                                         CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteSaleItemCommand for SaleItemId: {SaleItemId}", request.Id);

            var saleItem = await _saleItemRepository.GetByIdAsync(request.Id, cancellationToken);
            if (saleItem == null)
            {
                _logger.LogError("Sale item with ID {SaleItemId} not found.", request.Id);
                throw new NotFoundException($"Sale item with ID {request.Id} not found.");
            }

            await _saleItemRepository.DeleteAsync(saleItem.Id, cancellationToken);

            _logger.LogInformation("Sale item with ID {SaleItemId} deleted successfully.", request.Id);

            return new DeleteSaleItemResponse(true, $"Sale item with ID {request.Id} deleted successfully.");
        }
    }
}
