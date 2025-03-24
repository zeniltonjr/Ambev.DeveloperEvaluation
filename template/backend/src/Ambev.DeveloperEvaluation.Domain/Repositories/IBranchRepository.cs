﻿using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IBranchRepository
    {
        Task<Branch> CreateAsync(Branch branch, CancellationToken cancellationToken = default);
        Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Branch>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Branch> UpdateAsync(Branch branch, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Guid branchId, CancellationToken cancellationToken = default);
    }
}
