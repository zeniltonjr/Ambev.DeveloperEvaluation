using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;
using System.Security.Authentication;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResult>
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthenticateUserHandler> _logger;

        public AuthenticateUserHandler(IUserService userService, 
                                       IJwtTokenService jwtTokenService, 
                                       ILogger<AuthenticateUserHandler> logger)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<AuthenticateUserResult> Handle(AuthenticateUserCommand request, 
                                                         CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Attempting to authenticate user with email: {Email}", request.Email);

                var user = await _userService.AuthenticateAsync(request.Email, request.Password);

                var token = _jwtTokenService.GenerateToken(user);

                _logger.LogInformation("User authenticated successfully with email: {Email}", request.Email);

                return new AuthenticateUserResult
                {
                    Token = token,
                    Id = user.Id,
                    Name = user.Username,
                    Email = user.Email,
                    Phone = user.Phone,
                    Role = user.Role.ToString()
                };
            }
            catch (AuthenticationException ex)
            {
                _logger.LogError(ex, "Authentication failed for email: {Email}", request.Email);
                throw new UnauthorizedAccessException("Authentication failed: " + ex.Message);
            }
        }
    }
}
