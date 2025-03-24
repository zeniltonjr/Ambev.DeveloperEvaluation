using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.SaleItens.CreateSaleItem
{
    public class CreateSaleItemHandler : IRequestHandler<CreateSaleItemCommand, CreateSaleItemResult>
    {
        private readonly IMapper _mapper;
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CreateSaleItemHandler> _logger;

        public CreateSaleItemHandler(IMapper mapper,
                                     ISaleItemRepository saleItemRepository,
                                     IProductRepository productRepository,
                                     ILogger<CreateSaleItemHandler> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _saleItemRepository = saleItemRepository ?? throw new ArgumentNullException(nameof(saleItemRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CreateSaleItemResult> Handle(CreateSaleItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateSaleItemCommand for ProductId: {ProductId}, SaleId: {SaleId}", request.ProductId, request.SaleId);

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                _logger.LogError("Product with ID {ProductId} not found.", request.ProductId);
                throw new ArgumentException("Product not found.");
            }

            var saleItem = new SaleItem(request.SaleId,
                                        request.ProductId,
                                        request.Quantity,
                                        request.UnitPrice);

            await _saleItemRepository.CreateAsync(saleItem);

            _logger.LogInformation("Sale item created successfully with ID: {SaleItemId}", saleItem.Id);

            var result = new CreateSaleItemResult
            {
                Id = saleItem.Id,
                SaleId = saleItem.SaleId,
                ProductId = saleItem.ProductId,
                Quantity = saleItem.Quantity,
                UnitPrice = saleItem.UnitPrice,
                Discount = saleItem.Discount,
                TotalPrice = saleItem.TotalPrice
            };

            return result;
        }
    }
}
