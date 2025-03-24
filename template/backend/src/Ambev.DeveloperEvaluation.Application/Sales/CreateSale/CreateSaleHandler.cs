using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _salesRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IUserRepository _customerRepository;
        private readonly ILogger<CreateSaleHandler> _logger;

        public CreateSaleHandler(
            ISaleRepository salesRepository,
            IProductRepository productRepository,
            IBranchRepository branchRepository,
            IUserRepository customerRepository,
            ILogger<CreateSaleHandler> logger)
        {
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _branchRepository = branchRepository ?? throw new ArgumentNullException(nameof(branchRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand request, 
                                                   CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            _logger.LogInformation("Handling CreateSaleCommand for Sale with CustomerId {CustomerId} and BranchId {BranchId}",
                                    request.CustomerId, request.BranchId);

            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                _logger.LogError("Customer with ID {CustomerId} not found.", request.CustomerId);
                throw new Exception($"Customer with ID {request.CustomerId} not found.");
            }

            var branch = await _branchRepository.GetByIdAsync(request.BranchId);
            if (branch == null)
            {
                _logger.LogError("Branch with ID {BranchId} not found.", request.BranchId);
                throw new Exception($"Branch with ID {request.BranchId} not found.");
            }

            var saleId = Guid.NewGuid();
            var saleItems = new List<SaleItem>();

            foreach (var itemCommand in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemCommand.ProductId);
                if (product == null)
                {
                    _logger.LogError("Product with ID {ProductId} not found.", itemCommand.ProductId);
                    throw new Exception($"Product with ID {itemCommand.ProductId} not found.");
                }

                var saleItem = new SaleItem(
                    saleId,
                    itemCommand.ProductId,
                    itemCommand.Quantity,
                    itemCommand.UnitPrice
                );

                saleItems.Add(saleItem);
            }

            var sale = new Sale(
                request.SaleNumber,
                DateTime.UtcNow,
                request.CustomerId.ToString(),
                request.BranchId.ToString(),
                saleItems
            );

            SalesDomainService.ValidateSale(sale);

            await _salesRepository.CreateAsync(sale);

            _logger.LogInformation("Sale with ID {SaleId} created successfully.", sale.Id);

            return new CreateSaleResult(sale.Id);
        }
    }
}
