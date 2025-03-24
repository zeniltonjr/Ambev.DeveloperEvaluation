namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleResult
    {
        public Guid SaleId { get; }

        public UpdateSaleResult(Guid saleId)
        {
            SaleId = saleId;
        }
    }
}
