using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public class GetBranchHandler : IRequestHandler<GetBranchCommand, GetBranchResult>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBranchHandler> _logger;

        public GetBranchHandler(IBranchRepository branchRepository,
                                IMapper mapper,
                                ILogger<GetBranchHandler> logger)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetBranchResult> Handle(GetBranchCommand request,
                                                  CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetBranchCommand for Branch ID: {BranchId}", request.Id);

            var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);

            if (branch == null)
            {
                _logger.LogWarning("Branch with ID {BranchId} not found", request.Id);
                throw new NotFoundException($"Branch with ID {request.Id} not found.");
            }

            _logger.LogInformation("Branch with ID {BranchId} found", request.Id);

            return _mapper.Map<GetBranchResult>(branch);
        }
    }
}
