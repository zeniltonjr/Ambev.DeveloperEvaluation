using Ambev.DeveloperEvaluation.Application.Branchs.GetBranch;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<GetProductResult>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(IProductRepository productRepository,
                                          IMapper mapper,
                                          ILogger<GetAllProductsQueryHandler> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetProductResult>> Handle(GetAllProductsQuery request,
                                                         CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllProductsQuery with Skip: {Skip} and Take: {Take}", request.Skip, request.Take);

            var products = await _productRepository.GetAllAsync(cancellationToken);

            var pagedProducts = products.Skip(request.Skip).Take(request.Take);

            _logger.LogInformation("Returning {ProductCount} products", pagedProducts.Count());

            return _mapper.Map<List<GetProductResult>>(pagedProducts);
        }
    }
}
