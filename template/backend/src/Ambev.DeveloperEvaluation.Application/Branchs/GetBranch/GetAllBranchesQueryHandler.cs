using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branchs.GetBranch
{
    public class GetAllBranchesQueryHandler : IRequestHandler<GetAllBranchesQuery, List<GetBranchResult>>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllBranchesQueryHandler> _logger;

        public GetAllBranchesQueryHandler(IBranchRepository branchRepository,
                                          IMapper mapper,
                                          ILogger<GetAllBranchesQueryHandler> logger)
        {
            _branchRepository = branchRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetBranchResult>> Handle(GetAllBranchesQuery request,
                                                        CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllBranchesQuery with Skip: {Skip} and Take: {Take}", request.Skip, request.Take);

            var branches = await _branchRepository.GetAllAsync(cancellationToken);
            var result = _mapper.Map<List<GetBranchResult>>(branches.Skip(request.Skip).Take(request.Take));

            _logger.LogInformation("Retrieved {BranchCount} branches", result.Count);

            return result;
        }
    }
}
