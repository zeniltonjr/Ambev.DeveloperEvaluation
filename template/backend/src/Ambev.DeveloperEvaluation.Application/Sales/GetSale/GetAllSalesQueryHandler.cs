using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetAllSalesQueryHandler : IRequestHandler<GetAllSalesQuery, List<GetSaleResult>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllSalesQueryHandler> _logger;

        public GetAllSalesQueryHandler(ISaleRepository saleRepository,
                                       IMapper mapper,
                                       ILogger<GetAllSalesQueryHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<GetSaleResult>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllSalesQuery with Skip: {Skip}, Take: {Take}, SortBy: {SortBy}, SortOrder: {SortOrder}",
                                   request.Skip, request.Take, request.SortBy, request.SortOrder);

            var sales = _saleRepository.GetAllAsync(cancellationToken).Result.AsQueryable();

            IQueryable<Sale> sortedSales;
            switch (request.SortBy.ToLower())
            {
                case "saleDate":
                    sortedSales = request.SortOrder
                        ? sales.OrderBy(s => s.SaleDate)
                        : sales.OrderByDescending(s => s.SaleDate);
                    break;
                case "totalAmount":
                    sortedSales = request.SortOrder
                        ? sales.OrderBy(s => s.TotalAmount)
                        : sales.OrderByDescending(s => s.TotalAmount);
                    break;
                case "saleNumber":
                    sortedSales = request.SortOrder
                        ? sales.OrderBy(s => s.SaleNumber)
                        : sales.OrderByDescending(s => s.SaleNumber);
                    break;
                default:
                    sortedSales = request.SortOrder
                        ? sales.OrderBy(s => s.SaleDate)
                        : sales.OrderByDescending(s => s.SaleDate);
                    break;
            }

            var pagedSales = sortedSales.Skip(request.Skip).Take(request.Take).ToList();

            _logger.LogInformation("Found {SalesCount} sales after applying paging and sorting", pagedSales.Count);

            return _mapper.Map<List<GetSaleResult>>(pagedSales);
        }
    }
}
