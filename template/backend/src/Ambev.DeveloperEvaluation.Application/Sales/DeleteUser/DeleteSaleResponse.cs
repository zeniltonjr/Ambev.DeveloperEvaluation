namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser;

/// <summary>
/// Response model for DeleteUser operation
/// </summary>
public class DeleteSaleResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public DeleteSaleResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
