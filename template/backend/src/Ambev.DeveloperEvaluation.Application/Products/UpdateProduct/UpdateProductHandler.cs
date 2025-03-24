using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly ILogger<UpdateProductHandler> _logger;

        public UpdateProductHandler(IProductRepository productRepository,
                                    IBranchRepository branchRepository,
                                    ILogger<UpdateProductHandler> logger)
        {
            _productRepository = productRepository;
            _branchRepository = branchRepository;
            _logger = logger;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductCommand request,
                                                      CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateProductCommand for Product ID: {ProductId}", request.Id);

            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found.", request.Id);
                throw new NotFoundException($"Product with ID {request.Id} not found.");
            }

            if (request.BranchId != Guid.Empty)
            {
                var branchExists = await _branchRepository.AnyAsync(request.BranchId);
                if (!branchExists)
                {
                    _logger.LogWarning("Branch with ID {BranchId} not found.", request.BranchId);
                    throw new NotFoundException($"Branch with ID {request.BranchId} not found.");
                }

                product.BranchId = request.BranchId;
            }

            product.Name = request.Name;
            product.BasePrice = request.BasePrice;

            await _productRepository.UpdateAsync(product);

            _logger.LogInformation("Product with ID {ProductId} updated successfully.", request.Id);

            return new UpdateProductResult
            {
                Id = product.Id,
                Name = product.Name,
                BasePrice = product.BasePrice,
                BranchId = product.BranchId,
                Message = "Product updated successfully"
            };
        }
    }
}
