namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleResult
    {
        public Guid Id { get; set; }

        public CreateSaleResult(Guid id)
        {
            Id = id;
        }
    }
}
