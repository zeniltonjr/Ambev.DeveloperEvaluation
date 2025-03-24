using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public string Name { get; private set; }
        public List<Product> Products { get; set; } = new List<Product>();

        public Branch(string name)
        {
            Name = name;
        }

        public void SetName(string name)
        {
            Name = name;
        }
    }
}
