using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSalesQueryHandler : IRequestHandler<GetSalesQueryCommand, PaginatedSalesResponse>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSalesQueryHandler> _logger;

        public GetSalesQueryHandler(ISaleRepository saleRepository,
                                    IMapper mapper,
                                    ILogger<GetSalesQueryHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PaginatedSalesResponse> Handle(GetSalesQueryCommand request, 
                                                         CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetSalesQueryCommand with Customer: {Customer}, Branch: {Branch}, StartDate: {StartDate}, EndDate: {EndDate}, PageNumber: {PageNumber}, PageSize: {PageSize}",
                                   request.Customer, request.Branch, request.StartDate, request.EndDate, request.PageNumber, request.PageSize);

            var (sales, totalCount) = await _saleRepository.GetSalesAsync(
                request.Customer,
                request.Branch,
                request.StartDate,
                request.EndDate,
                request.PageNumber,
                request.PageSize
            );

            var salesResponse = _mapper.Map<List<GetSaleResponse>>(sales);

            _logger.LogInformation("Found {TotalCount} sales for the specified criteria", totalCount);

            return new PaginatedSalesResponse(salesResponse, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
