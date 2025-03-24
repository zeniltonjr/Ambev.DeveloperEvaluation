using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users
{
    /// <summary>
    /// Controller for managing user operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// Initializes a new instance of UsersController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="logger">The logger instance</param>
        public UsersController(IMediator mediator,
                               IMapper mapper,
                               ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="request">The user creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created user details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateUserResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request,
                                                    CancellationToken cancellationToken)
        {
            var validator = new CreateUserRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("User creation failed due to validation errors.");
                return BadRequest(validationResult.Errors);
            }

            var command = _mapper.Map<CreateUserCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("User created successfully with ID {UserId}.", response.Id);

            return Created(string.Empty, new ApiResponseWithData<CreateUserResponse>
            {
                Success = true,
                Message = "User created successfully",
                Data = _mapper.Map<CreateUserResponse>(response)
            });
        }

        /// <summary>
        /// Retrieves a user by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the user</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The user details if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromRoute] Guid id,
                                                  CancellationToken cancellationToken)
        {
            var request = new GetUserRequest { Id = id };
            var validator = new GetUserRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Failed to retrieve user with ID {UserId} due to validation errors.", id);
                return BadRequest(validationResult.Errors);
            }

            var command = _mapper.Map<GetUserCommand>(request.Id);
            var response = await _mediator.Send(command, cancellationToken);

            if (response == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                return NotFound(new ApiResponse { Success = false, Message = "User not found" });
            }

            _logger.LogInformation("User with ID {UserId} retrieved successfully.", id);

            return Ok(new ApiResponseWithData<GetUserResponse>
            {
                Success = true,
                Message = "User retrieved successfully",
                Data = _mapper.Map<GetUserResponse>(response)
            });
        }

        /// <summary>
        /// Deletes a user by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Success response if the user was deleted</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id,
                                                    CancellationToken cancellationToken)
        {
            var request = new DeleteUserRequest { Id = id };
            var validator = new DeleteUserRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Failed to delete user with ID {UserId} due to validation errors.", id);
                return BadRequest(validationResult.Errors);
            }

            var command = _mapper.Map<DeleteUserCommand>(request.Id);
            await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("User with ID {UserId} deleted successfully.", id);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "User deleted successfully"
            });
        }
    }
}
