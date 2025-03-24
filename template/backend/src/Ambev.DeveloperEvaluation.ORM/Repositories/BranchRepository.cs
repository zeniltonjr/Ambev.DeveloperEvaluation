using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly DefaultContext _context;
        private readonly ILogger<BranchRepository> _logger;

        public BranchRepository(DefaultContext context, 
                                ILogger<BranchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Branch> CreateAsync(Branch branch, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating a new branch with name: {BranchName}", branch.Name);

            await _context.Branches.AddAsync(branch, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Branch created successfully with ID: {BranchId}", branch.Id);

            return branch;
        }

        public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching branch with ID: {BranchId}", id);

            var branch = await _context.Branches
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

            if (branch != null)
                _logger.LogInformation("Branch found with ID: {BranchId}", id);
            else
                _logger.LogWarning("Branch not found with ID: {BranchId}", id);

            return branch;
        }

        public async Task<IEnumerable<Branch>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching all branches.");

            var branches = await _context.Branches
                .Include(b => b.Products)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Fetched {BranchCount} branches.", branches.Count());

            return branches;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to delete branch with ID: {BranchId}", id);

            var branch = await GetByIdAsync(id, cancellationToken);
            if (branch == null)
            {
                _logger.LogWarning("Branch not found with ID: {BranchId}", id);
                return false;
            }

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Branch with ID: {BranchId} deleted successfully.", id);

            return true;
        }

        public async Task<Branch> UpdateAsync(Branch branch, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to update branch with ID: {BranchId}", branch.Id);

            var existingBranch = await GetByIdAsync(branch.Id, cancellationToken);
            if (existingBranch == null)
            {
                _logger.LogWarning("Branch with ID: {BranchId} not found for update.", branch.Id);
                throw new KeyNotFoundException($"Branch with ID {branch.Id} not found.");
            }

            existingBranch.SetName(branch.Name);

            _context.Branches.Update(existingBranch);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Branch with ID: {BranchId} updated successfully.", branch.Id);

            return existingBranch;
        }

        public async Task<bool> AnyAsync(Guid branchId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Checking if branch exists with ID: {BranchId}", branchId);

            var exists = await _context.Branches
                .AnyAsync(b => b.Id == branchId, cancellationToken);

            _logger.LogInformation("Branch exists with ID: {BranchId}: {Exists}", branchId, exists);

            return exists;
        }
    }
}
