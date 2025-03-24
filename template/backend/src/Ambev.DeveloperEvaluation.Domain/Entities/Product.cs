using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product(string name, 
                       decimal basePrice, 
                       Guid branchId)
        {
            Name = name;
            BasePrice = basePrice;
            BranchId = branchId;
        }

        public string Name { get; set; } = string.Empty;

        public decimal BasePrice { get; set; }

        public Guid BranchId { get; set; }

        public Branch Branch { get; set; }
    }
}
