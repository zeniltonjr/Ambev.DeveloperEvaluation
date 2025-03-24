namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.UpdateSaleItem
{
    public class UpdateSaleItemResponse
    {
        public Guid SaleItemId { get; set; }      

        public Guid ProductId { get; set; }    
        
        public int Quantity { get; set; }        

        public decimal UnitPrice { get; set; }    

        public decimal Discount { get; set; }     

        public decimal TotalPrice { get; set; }   

        public bool IsSuccess { get; set; }       

        public string Message { get; set; }
    }
}
