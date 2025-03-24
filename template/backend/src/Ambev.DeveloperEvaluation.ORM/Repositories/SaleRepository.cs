using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;
        private readonly ILogger<SaleRepository> _logger;

        public SaleRepository(DefaultContext context, 
                              ILogger<SaleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Sale> CreateAsync(Sale sale,
                                            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating sale with ID: {SaleId}", sale.Id);

            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Sale created with ID: {SaleId}", sale.Id);
            return sale;
        }

        public async Task<Sale?> GetByIdAsync(Guid id,
                                              CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching sale with ID: {SaleId}", id);

            var sale = await _context.Sales
                .Include(s => s.SaleItens)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if (sale != null)
            {
                _logger.LogInformation("Sale found with ID: {SaleId}", id);
            }
            else
            {
                _logger.LogWarning("Sale not found with ID: {SaleId}", id);
            }

            return sale;
        }

        public async Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching all sales.");

            var sales = await _context.Sales
                .Include(s => s.SaleItens)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Fetched {SalesCount} sales.", sales.Count());
            return sales;
        }

        public async Task<bool> DeleteAsync(Guid id,
                                            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to delete sale with ID: {SaleId}", id);

            var sale = await GetByIdAsync(id, cancellationToken);
            if (sale == null)
            {
                _logger.LogWarning("Sale not found with ID: {SaleId}", id);
                return false;
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Sale with ID: {SaleId} deleted successfully.", id);
            return true;
        }

        public async Task<(List<Sale> Sales, int TotalCount)> GetSalesAsync(string? customer,
                                                                            string? branch,
                                                                            DateTime? startDate,
                                                                            DateTime? endDate,
                                                                            int pageNumber,
                                                                            int pageSize,
                                                                            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching sales with filters. Customer: {Customer}, Branch: {Branch}, Start Date: {StartDate}, End Date: {EndDate}, Page: {PageNumber}, Page Size: {PageSize}",
                                    customer, branch, startDate, endDate, pageNumber, pageSize);

            var query = _context.Sales.AsQueryable();

            if (!string.IsNullOrEmpty(customer))
                query = query.Where(s => s.Customer.Username.Contains(customer));

            if (!string.IsNullOrEmpty(branch))
                query = query.Where(s => s.Branch.Name.Contains(branch));

            if (startDate.HasValue)
                query = query.Where(s => s.SaleDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(s => s.SaleDate <= endDate.Value);

            var totalCount = await query.CountAsync(cancellationToken);
            _logger.LogInformation("Total sales count: {TotalCount}", totalCount);

            var sales = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Fetched {SalesCount} sales for the given filters.", sales.Count());
            return (sales, totalCount);
        }

        public async Task<Sale> UpdateAsync(Sale sale,
                                            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to update sale with ID: {SaleId}", sale.Id);

            var existingSale = await GetByIdAsync(sale.Id, cancellationToken);
            if (existingSale == null)
                throw new NotFoundException($"Sale with ID {sale.Id} not found.");

            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Sale with ID: {SaleId} updated successfully.", sale.Id);
            return sale;
        }
    }
}
