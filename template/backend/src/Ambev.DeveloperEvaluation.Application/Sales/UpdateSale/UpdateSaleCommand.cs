namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommand
    {
        public Guid SaleId { get; set; } 

        public string SaleNumber { get; set; } 

        public Guid CustomerId { get; set; } 

        public Guid BranchId { get; set; } 

        public List<SaleItemCommand> Items { get; set; } 

        public UpdateSaleCommand(Guid saleId, 
                                 string saleNumber, 
                                 Guid customerId, Guid branchId, 
                                 List<SaleItemCommand> items)
        {
            SaleId = saleId;
            SaleNumber = saleNumber;
            CustomerId = customerId;
            BranchId = branchId;
            Items = items ?? new List<SaleItemCommand>();
        }
    }

    public class SaleItemCommand
    {
        public Guid ProductId { get; set; } 

        public int Quantity { get; set; } 

        public decimal UnitPrice { get; set; }

        public SaleItemCommand(Guid productId,
                               int quantity, 
                               decimal unitPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
