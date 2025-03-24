namespace Ambev.DeveloperEvaluation.Application.SaleItens.DeleteSaleItem
{
    public class DeleteSaleItemResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public DeleteSaleItemResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
