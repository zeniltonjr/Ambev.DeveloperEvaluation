using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.SaleItens.GetSaleItem
{
    public class GetSaleItemHandler : IRequestHandler<GetSaleItemCommand, GetSaleItemResult>
    {
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSaleItemHandler> _logger;

        public GetSaleItemHandler(ISaleItemRepository saleItemRepository,
                                 IMapper mapper,
                                 ILogger<GetSaleItemHandler> logger)
        {
            _saleItemRepository = saleItemRepository;
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GetSaleItemResult> Handle(GetSaleItemCommand request,
                                                    CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetSaleItemCommand for SaleItem with ID {SaleItemId}", request.Id);

            var saleItem = await _saleItemRepository.GetByIdAsync(request.Id, cancellationToken);

            if (saleItem == null)
            {
                _logger.LogWarning("SaleItem with ID {SaleItemId} not found.", request.Id);
                throw new NotFoundException($"Sale item with ID {request.Id} not found.");
            }

            _logger.LogInformation("SaleItem with ID {SaleItemId} found, returning result.", request.Id);

            return _mapper.Map<GetSaleItemResult>(saleItem);
        }
    }
}
