using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Branchs.DeleteBranch
{
    public class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, Unit>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly ILogger<DeleteBranchHandler> _logger;

        public DeleteBranchHandler(IBranchRepository branchRepository, 
                                   ILogger<DeleteBranchHandler> logger)
        {
            _branchRepository = branchRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteBranchCommand request,
                                       CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteBranchCommand for branch ID: {BranchId}", request.Id);

            var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);
            if (branch == null)
            {
                _logger.LogWarning("Branch with ID {BranchId} not found", request.Id);
                throw new NotFoundException($"Branch with ID {request.Id} not found.");
            }

            _logger.LogInformation("Deleting branch with ID: {BranchId}", request.Id);

            await _branchRepository.DeleteAsync(branch.Id, cancellationToken);

            _logger.LogInformation("Branch with ID {BranchId} deleted successfully", request.Id);

            return Unit.Value;
        }
    }
}
