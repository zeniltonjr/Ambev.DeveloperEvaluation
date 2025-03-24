using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.DeleteUser
{
    /// <summary>
    /// Handler for processing DeleteUserCommand requests
    /// </summary>
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteUserHandler> _logger;

        /// <summary>
        /// Initializes a new instance of DeleteUserHandler
        /// </summary>
        /// <param name="userRepository">The user repository</param>
        /// <param name="logger">The logger</param>
        public DeleteUserHandler(IUserRepository userRepository,
                                 ILogger<DeleteUserHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Handles the DeleteUserCommand request
        /// </summary>
        /// <param name="request">The DeleteUser command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result of the delete operation</returns>
        public async Task<DeleteUserResponse> Handle(DeleteUserCommand request,
                                                     CancellationToken cancellationToken)
        {
            var validator = new DeleteUserValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for DeleteUserCommand. Errors: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(validationResult.Errors);
            }

            var success = await _userRepository.DeleteAsync(request.Id, cancellationToken);
            if (!success)
            {
                _logger.LogWarning("Attempt to delete non-existing user with ID: {UserId}", request.Id);
                throw new KeyNotFoundException($"User with ID {request.Id} not found");
            }

            _logger.LogInformation("User with ID {UserId} deleted successfully", request.Id);

            return new DeleteUserResponse { Success = true };
        }
    }
}
