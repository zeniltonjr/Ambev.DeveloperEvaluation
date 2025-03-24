using Ambev.DeveloperEvaluation.Domain.Entities;

public interface IUserService
{
    Task<User> AuthenticateAsync(string email, string password);
}