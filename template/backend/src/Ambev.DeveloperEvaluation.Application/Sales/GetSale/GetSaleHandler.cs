using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSaleHandler> _logger;

        public GetSaleHandler(ISaleRepository saleRepository,
                              IUserRepository userRepository,
                              IBranchRepository branchRepository,
                              IMapper mapper,
                              ILogger<GetSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _userRepository = userRepository;
            _branchRepository = branchRepository;
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GetSaleResult> Handle(GetSaleCommand request,
                                                CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetSaleCommand for Sale ID: {SaleId}", request.Id);

            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (sale == null)
            {
                _logger.LogWarning("Sale with ID {SaleId} not found", request.Id);
                throw new NotFoundException($"Sale with ID {request.Id} not found.");
            }

            _logger.LogInformation("Sale with ID {SaleId} found", request.Id);

            return _mapper.Map<GetSaleResult>(sale);
        }
    }
}
