using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DeleteProductHandler> _logger;

        public DeleteProductHandler(IProductRepository productRepository, ILogger<DeleteProductHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<DeleteProductResponse> Handle(DeleteProductCommand request,
                                                         CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteProductCommand for Product ID: {ProductId}", request.Id);

            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found.", request.Id);
                throw new NotFoundException($"Product with ID {request.Id} not found.");
            }

            await _productRepository.DeleteAsync(product.Id, cancellationToken);

            _logger.LogInformation("Product with ID {ProductId} deleted successfully.", request.Id);

            return new DeleteProductResponse(true, $"Product with ID {request.Id} deleted successfully.");
        }
    }
}
