using Ambev.DeveloperEvaluation.Application.Branchs.CreateBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CreateProductHandler> _logger;

        public CreateProductHandler(IProductRepository branchRepository, 
                                    ILogger<CreateProductHandler> logger)
        {
            _productRepository = branchRepository;
            _logger = logger;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand request, 
                                                      CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateProductCommand for Product Name: {ProductName}", request.Name);

            Product product = new Product(request.Name, request.BasePrice, request.BranchId);

            var savedProduct = await _productRepository.CreateAsync(product, cancellationToken);

            _logger.LogInformation("Product {ProductName} created successfully with ID: {ProductId}", savedProduct.Name, savedProduct.Id);

            var result = new CreateProductResult
            {
                Name = savedProduct.Name,
                BasePrice = savedProduct.BasePrice,
                BranchId = savedProduct.BranchId
            };

            return result;
        }
    }
}
