using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
