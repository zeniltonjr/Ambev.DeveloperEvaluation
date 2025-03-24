using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branchs.CreateBranch
{
    public class CreateBranchHandler : IRequestHandler<CreateBranchCommand, CreateBranchResult>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly ILogger<CreateBranchHandler> _logger;

        public CreateBranchHandler(IBranchRepository branchRepository, 
                                   ILogger<CreateBranchHandler> logger)
        {
            _branchRepository = branchRepository;
            _logger = logger;
        }

        public async Task<CreateBranchResult> Handle(CreateBranchCommand request, 
                                                     CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateBranchCommand for branch name: {BranchName}", request.Name);

            var branch = new Branch(request.Name);

            _logger.LogInformation("Creating new branch with name: {BranchName}", request.Name);

            var savedBranch = await _branchRepository.CreateAsync(branch);

            var result = new CreateBranchResult
            {
                Id = savedBranch.Id,
                Name = savedBranch.Name
            };

            _logger.LogInformation("Branch with name {BranchName} created successfully", savedBranch.Name);

            return result;
        }
    }
}
