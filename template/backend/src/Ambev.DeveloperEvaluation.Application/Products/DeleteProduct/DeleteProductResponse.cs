namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public class DeleteProductResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public DeleteProductResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
