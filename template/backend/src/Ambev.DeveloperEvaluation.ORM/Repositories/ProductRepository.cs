using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DefaultContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(DefaultContext context, 
                                 ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Product> CreateAsync(Product product,
                                               CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating a new product with name: {ProductName}", product.Name);

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);
            return product;
        }

        public async Task<Product?> GetByIdAsync(Guid id,
                                                 CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching product with ID: {ProductId}", id);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (product != null)
                _logger.LogInformation("Product found with ID: {ProductId}", id);
            else
                _logger.LogWarning("Product not found with ID: {ProductId}", id);
            

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Fetching all products.");

            var products = await _context.Products.ToListAsync(cancellationToken);
            _logger.LogInformation("Fetched {ProductCount} products.", products.Count());

            return products;
        }

        public async Task<bool> DeleteAsync(Guid id,
                                            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to delete product with ID: {ProductId}", id);

            var product = await GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                _logger.LogWarning("Product not found with ID: {ProductId}", id);
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product with ID: {ProductId} deleted successfully.", id);
            return true;
        }

        public async Task<Product> UpdateAsync(Product product,
                                               CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Attempting to update product with ID: {ProductId}", product.Id);

            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product with ID: {ProductId} updated successfully.", product.Id);
            return product;
        }
    }
}
