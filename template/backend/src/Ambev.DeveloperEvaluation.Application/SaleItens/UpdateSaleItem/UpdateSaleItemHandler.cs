using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.SaleItens.UpdateSaleItem
{
    public class UpdateSaleItemHandler : IRequestHandler<UpdateSaleItemCommand, UpdateSaleItemResult>
    {
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<UpdateSaleItemHandler> _logger;

        public UpdateSaleItemHandler(
            ISaleItemRepository saleItemRepository,
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            ILogger<UpdateSaleItemHandler> logger)
        {
            _saleItemRepository = saleItemRepository;
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UpdateSaleItemResult> Handle(UpdateSaleItemCommand request,
                                                       CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateSaleItemCommand for SaleItemId: {SaleItemId}", request.SaleItemId);

            var sale = await _saleRepository.GetByIdAsync(request.SaleId);
            if (sale == null)
            {
                _logger.LogError("Sale with ID {SaleId} not found.", request.SaleId);
                throw new NotFoundException($"Sale with ID {request.SaleId} not found.");
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                _logger.LogError("Product with ID {ProductId} not found.", request.ProductId);
                throw new NotFoundException($"Product with ID {request.ProductId} not found.");
            }

            var saleItem = await _saleItemRepository.GetByIdAsync(request.SaleItemId);

            if (saleItem == null)
            {
                _logger.LogError("SaleItem with ID {SaleItemId} not found.", request.SaleItemId);
                throw new NotFoundException($"SaleItem with ID {request.SaleItemId} not found.");
            }

            if (saleItem.SaleId != request.SaleId || saleItem.ProductId != request.ProductId)
            {
                _logger.LogError("Mismatch between SaleId or ProductId for SaleItem with ID {SaleItemId}.", request.SaleItemId);
                throw new DomainException("The SaleId or ProductId does not match the existing SaleItem.");
            }

            saleItem.UpdateQuantity(request.Quantity);
            saleItem.UpdateUnitPrice(request.UnitPrice);
            saleItem.UpdateTotalPrice();

            await _saleItemRepository.UpdateAsync(saleItem);

            _logger.LogInformation("SaleItem with ID {SaleItemId} updated successfully.", request.SaleItemId);

            return new UpdateSaleItemResult
            {
                SaleItemId = saleItem.Id,
                Quantity = saleItem.Quantity,
                UnitPrice = saleItem.UnitPrice,
                Discount = saleItem.Discount,
                TotalPrice = saleItem.TotalPrice,
                Message = "Sale Item updated successfully"
            };
        }
    }
}
