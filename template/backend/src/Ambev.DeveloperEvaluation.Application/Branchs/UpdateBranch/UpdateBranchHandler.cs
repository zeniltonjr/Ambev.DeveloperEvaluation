using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch
{
    public class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, UpdateBranchResult>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly ILogger<UpdateBranchHandler> _logger;

        public UpdateBranchHandler(IBranchRepository branchRepository,
                                   ILogger<UpdateBranchHandler> logger)
        {
            _branchRepository = branchRepository;
            _logger = logger;
        }

        public async Task<UpdateBranchResult> Handle(UpdateBranchCommand request,
                                                     CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateBranchCommand for branch ID: {BranchId}", request.Id);

            var branch = await _branchRepository.GetByIdAsync(request.Id);
            if (branch == null)
            {
                _logger.LogWarning("Branch with ID {BranchId} not found", request.Id);
                throw new NotFoundException($"Branch with ID {request.Id} not found.");
            }

            branch.SetName(request.Name);
            await _branchRepository.UpdateAsync(branch);

            _logger.LogInformation("Branch with ID {BranchId} updated successfully", request.Id);

            return new UpdateBranchResult
            {
                Id = branch.Id,
                Name = branch.Name,
                Message = "Branch updated successfully"
            };
        }
    }
}
