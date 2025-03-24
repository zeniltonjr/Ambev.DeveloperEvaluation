using Ambev.DeveloperEvaluation.Application.Branchs.GetBranch;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductHandler : IRequestHandler<GetProductCommand, GetProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductHandler> _logger;

        public GetProductHandler(IProductRepository productRepository, 
                                 IMapper mapper, 
                                 ILogger<GetProductHandler> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetProductResult> Handle(GetProductCommand request,
                                                   CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetProductCommand for Product ID: {ProductId}", request.Id);

            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found.", request.Id);
                throw new NotFoundException($"Product with ID {request.Id} not found.");
            }

            _logger.LogInformation("Product with ID {ProductId} found successfully.", request.Id);

            return _mapper.Map<GetProductResult>(product); 
        }
    }
}
