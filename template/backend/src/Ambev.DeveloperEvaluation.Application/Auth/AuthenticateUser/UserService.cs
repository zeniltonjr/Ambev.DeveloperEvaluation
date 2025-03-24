using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using System.Security.Authentication;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository,
                           ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User> AuthenticateAsync(string email, 
                                                  string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                _logger.LogWarning("Authentication failed: Email or password is empty.");
                throw new AuthenticationException("Email or password cannot be empty.");
            }

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("Authentication failed: User not found for email {Email}", email);
                throw new AuthenticationException("Invalid credentials: User not found.");
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Authentication failed: Incorrect password for user {Email}", email);
                throw new AuthenticationException("Invalid credentials: Incorrect password.");
            }

            _logger.LogInformation("User authenticated successfully: {Email}", email);

            return user;
        }
    }
}
