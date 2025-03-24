using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser
{
    /// <summary>
    /// Handler for processing GetUserCommand requests
    /// </summary>
    public class GetUserHandler : IRequestHandler<GetUserCommand, GetUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserHandler> _logger;

        /// <summary>
        /// Initializes a new instance of GetUserHandler
        /// </summary>
        /// <param name="userRepository">The user repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="logger">The logger</param>
        public GetUserHandler(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<GetUserHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Handles the GetUserCommand request
        /// </summary>
        /// <param name="request">The GetUser command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The user details if found</returns>
        public async Task<GetUserResult> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetUserValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for GetUserCommand. Errors: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", request.Id);
                throw new KeyNotFoundException($"User with ID {request.Id} not found");
            }

            _logger.LogInformation("User with ID {UserId} found", request.Id);

            return _mapper.Map<GetUserResult>(user);
        }
    }
}
