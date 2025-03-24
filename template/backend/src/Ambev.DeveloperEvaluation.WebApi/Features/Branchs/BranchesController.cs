using Ambev.DeveloperEvaluation.Application.Branchs.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Branchs.DeleteBranch;
using Ambev.DeveloperEvaluation.Application.Branchs.GetBranch;
using Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.CreateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.DeleteBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.GetBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.GetUpdate;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branchs
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BranchesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BranchesController> _logger;

        public BranchesController(IMediator mediator,
                                  IMapper mapper,
                                  ILogger<BranchesController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<List<GetBranchResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllBranches(CancellationToken cancellationToken,
                                                        [FromQuery] int? _page = null,
                                                        [FromQuery] int? _size = null)
        {
            try
            {
                int page = _page ?? 1;
                int size = _size ?? 10;

                if (page <= 0 || size <= 0)
                {
                    _logger.LogWarning("Invalid page or size parameters: page = {Page}, size = {Size}", page, size);
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Page number and size must be greater than zero."
                    });
                }

                _logger.LogInformation("Fetching all branches with pagination: page = {Page}, size = {Size}", page, size);

                var commandWithPagination = new GetAllBranchesQuery
                {
                    Skip = (page - 1) * size,
                    Take = size
                };

                var responsePaged = await _mediator.Send(commandWithPagination, cancellationToken);
                var resultPaged = _mapper.Map<List<GetBranchResponse>>(responsePaged);

                _logger.LogInformation("Successfully retrieved {Count} branches", resultPaged.Count);

                return Ok(new ApiResponseWithData<List<GetBranchResponse>>
                {
                    Success = true,
                    Message = "Branches retrieved successfully",
                    Data = resultPaged
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving branches");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateBranchResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchRequest request,
                                                      CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateBranchRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Invalid CreateBranchRequest: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    return BadRequest(validationResult.Errors);
                }

                var command = _mapper.Map<CreateBranchCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Branch created successfully with ID: {BranchId}", response.Id);

                return Created(string.Empty, new ApiResponseWithData<CreateBranchResponse>
                {
                    Success = true,
                    Message = "Branch created successfully",
                    Data = _mapper.Map<CreateBranchResponse>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the branch");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetBranchResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBranch(Guid id,
                                                  CancellationToken cancellationToken)
        {
            try
            {
                var request = new GetBranchRequest { Id = id };
                var validator = new GetBranchRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Invalid GetBranchRequest: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    return BadRequest(validationResult.Errors);
                }

                var command = _mapper.Map<GetBranchCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                var mappedResponse = _mapper.Map<GetBranchResponse>(response);

                _logger.LogInformation("Branch with ID {BranchId} retrieved successfully", id);

                return Ok(new ApiResponseWithData<GetBranchResponse>
                {
                    Success = true,
                    Message = "Branch retrieved successfully",
                    Data = mappedResponse
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving branch with ID {BranchId}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBranch(Guid id,
                                                     CancellationToken cancellationToken)
        {
            try
            {
                var request = new DeleteBranchRequest { Id = id };
                var validator = new DeleteBranchRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Invalid DeleteBranchRequest: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    return BadRequest(validationResult.Errors);
                }

                var command = _mapper.Map<DeleteBranchCommand>(id);
                await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Branch with ID {BranchId} deleted successfully", id);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Branch deleted successfully",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting branch with ID {BranchId}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateBranchResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBranch(Guid id, [FromBody] UpdateBranchRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UpdateBranchRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Invalid UpdateBranchRequest: {Errors}", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    return BadRequest(validationResult.Errors);
                }

                var command = _mapper.Map<UpdateBranchCommand>(request);
                command.Id = id;

                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Branch with ID {BranchId} updated successfully", id);

                return Ok(new ApiResponseWithData<UpdateBranchResponse>
                {
                    Success = true,
                    Message = "Branch updated successfully",
                    Data = _mapper.Map<UpdateBranchResponse>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating branch with ID {BranchId}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }
    }
}
