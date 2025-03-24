using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleItemRepository : ISaleItemRepository
    {
        private readonly DefaultContext _context;
        private readonly ILogger<SaleItemRepository> _logger;

        public SaleItemRepository(DefaultContext context, ILogger<SaleItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SaleItem> CreateAsync(SaleItem saleItem,
                                                CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating sale item with SaleId: {SaleId} and ProductId: {ProductId}", saleItem.SaleId, saleItem.ProductId);

            await _context.SaleItems.AddAsync(saleItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Sale item created with ID: {SaleItemId}", saleItem.Id);
            return saleItem;
        }

        public async Task<SaleItem?> GetByIdAsync(Guid id,
                                                  CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching sale item with ID: {SaleItemId}", id);

            var saleItem = await _context.SaleItems
                .FirstOrDefaultAsync(si => si.Id == id, cancellationToken);

            if (saleItem != null)
                _logger.LogInformation("Sale item found with ID: {SaleItemId}", id);
            else
                _logger.LogWarning("Sale item not found with ID: {SaleItemId}", id);

            return saleItem;
        }

        public async Task<IEnumerable<SaleItem>> GetBySaleIdAsync(Guid saleId,
                                                                  CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching sale items with SaleId: {SaleId}", saleId);

            var saleItems = await _context.SaleItems
                .Where(si => si.SaleId == saleId)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Fetched {SaleItemCount} sale items for SaleId: {SaleId}", saleItems.Count(), saleId);
            return saleItems;
        }

        public async Task<bool> DeleteAsync(Guid id,
                                            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to delete sale item with ID: {SaleItemId}", id);

            var saleItem = await GetByIdAsync(id, cancellationToken);
            if (saleItem == null)
            {
                _logger.LogWarning("Sale item not found with ID: {SaleItemId}", id);
                return false;
            }

            _context.SaleItems.Remove(saleItem);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Sale item with ID: {SaleItemId} deleted successfully.", id);
            return true;
        }

        public async Task<IEnumerable<SaleItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching all sale items.");

            var saleItems = await _context.SaleItems.ToListAsync(cancellationToken);
            _logger.LogInformation("Fetched {SaleItemCount} sale items.", saleItems.Count());

            return saleItems;
        }

        public async Task<SaleItem?> UpdateAsync(SaleItem saleItem, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to update sale item with ID: {SaleItemId}", saleItem.Id);

            var existingSaleItem = await _context.SaleItems
                .FirstOrDefaultAsync(si => si.Id == saleItem.Id, cancellationToken);

            if (existingSaleItem == null)
            {
                _logger.LogWarning("Sale item not found with ID: {SaleItemId}", saleItem.Id);
                return null;
            }

            existingSaleItem.UpdateSaleItem(saleItem.Quantity, saleItem.UnitPrice);

            _context.SaleItems.Update(existingSaleItem);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Sale item with ID: {SaleItemId} updated successfully.", saleItem.Id);
            return existingSaleItem;
        }
    }
}
