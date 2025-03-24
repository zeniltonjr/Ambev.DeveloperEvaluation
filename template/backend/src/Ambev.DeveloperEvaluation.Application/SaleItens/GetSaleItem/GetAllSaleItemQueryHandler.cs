using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.SaleItens.GetSaleItem
{
    public class GetAllSaleItemQueryHandler : IRequestHandler<GetAllSaleItemsQuery, List<GetSaleItemResult>>
    {
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllSaleItemQueryHandler> _logger;

        public GetAllSaleItemQueryHandler(ISaleItemRepository saleItemRepository,
                                          IMapper mapper,
                                          ILogger<GetAllSaleItemQueryHandler> logger)
        {
            _saleItemRepository = saleItemRepository;
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<GetSaleItemResult>> Handle(GetAllSaleItemsQuery request,
                                                          CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllSaleItemsQuery with Skip: {Skip} and Take: {Take}", request.Skip, request.Take);

            var products = await _saleItemRepository.GetAllAsync(cancellationToken);

            var pagedProducts = products.Skip(request.Skip).Take(request.Take);

            _logger.LogInformation("Returning {Count} sale items.", pagedProducts.Count());

            return _mapper.Map<List<GetSaleItemResult>>(pagedProducts);
        }
    }
}
