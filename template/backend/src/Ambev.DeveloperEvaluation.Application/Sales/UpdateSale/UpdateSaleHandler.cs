using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleItemCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _salesRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IUserRepository _customerRepository;
        private readonly ILogger<UpdateSaleHandler> _logger;

        public UpdateSaleHandler(
            ISaleRepository salesRepository,
            IProductRepository productRepository,
            IBranchRepository branchRepository,
            IUserRepository customerRepository,
            ILogger<UpdateSaleHandler> logger)
        {
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _branchRepository = branchRepository ?? throw new ArgumentNullException(nameof(branchRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleItemCommand request,
                                                   CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            _logger.LogInformation("Handling UpdateSaleItemCommand for Sale ID: {SaleId}", request.SaleId);

            var sale = await _salesRepository.GetByIdAsync(request.SaleId);
            if (sale == null)
            {
                _logger.LogWarning("Sale with ID {SaleId} not found", request.SaleId);
                throw new Exception($"Sale with ID {request.SaleId} not found.");
            }

            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found", request.CustomerId);
                throw new Exception($"Customer with ID {request.CustomerId} not found.");
            }

            var branch = await _branchRepository.GetByIdAsync(request.BranchId);
            if (branch == null)
            {
                _logger.LogWarning("Branch with ID {BranchId} not found", request.BranchId);
                throw new Exception($"Branch with ID {request.BranchId} not found.");
            }

            var updatedSaleItems = new List<SaleItem>();
            foreach (var itemCommand in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemCommand.ProductId);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", itemCommand.ProductId);
                    throw new Exception($"Product with ID {itemCommand.ProductId} not found.");
                }

                var saleItem = new SaleItem(
                    sale.Id,
                    itemCommand.ProductId,
                    itemCommand.Quantity,
                    itemCommand.UnitPrice
                );

                updatedSaleItems.Add(saleItem);
            }

            sale.UpdateSale(request.SaleNumber,
                            request.CustomerId.ToString(),
                            request.BranchId.ToString(),
                            updatedSaleItems);

            SalesDomainService.ValidateSale(sale);

            await _salesRepository.UpdateAsync(sale);

            _logger.LogInformation("Sale with ID {SaleId} updated successfully", sale.Id);

            return new UpdateSaleResult(sale.Id);
        }
    }
}
