using Ambev.DeveloperEvaluation.Application.SaleItens.CreateSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItens.DeleteSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItens.GetSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItens.UpdateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.CreateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.DeleteSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.GetSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.UpdateSaleItem;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItens
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SaleItemController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<SaleItemController> _logger;

        public SaleItemController(ILogger<SaleItemController> logger,
                                  IMediator mediator,
                                  IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger; // Initialize logger
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleItemResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSaleItem([FromBody] CreateSaleItemRequest request,
                                                        CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to create a sale item with product ID: {ProductId}", request.ProductId);

            var validator = new CreateSaleItemRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for product ID: {ProductId}. Errors: {Errors}", request.ProductId, validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var command = _mapper.Map<CreateSaleItemCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Sale item created successfully with product ID: {ProductId}", request.ProductId);

                return Created(string.Empty, new ApiResponseWithData<CreateSaleItemResponse>
                {
                    Success = true,
                    Message = "Sale item created successfully",
                    Data = _mapper.Map<CreateSaleItemResponse>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating sale item with product ID: {ProductId}", request.ProductId);
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSaleItem(Guid id,
                                                     CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to get sale item with ID: {SaleItemId}", id);

            try
            {
                var request = new GetSaleItemRequest { Id = id };
                var command = _mapper.Map<GetSaleItemCommand>(request);

                var response = await _mediator.Send(command, cancellationToken);

                if (response == null)
                {
                    _logger.LogWarning("Sale item with ID: {SaleItemId} not found", id);
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = $"Sale item with ID {id} not found"
                    });
                }

                _logger.LogInformation("Sale item with ID: {SaleItemId} retrieved successfully", id);
                return Ok(new ApiResponseWithData<GetSaleItemResponse>
                {
                    Success = true,
                    Message = "Sale item retrieved successfully",
                    Data = _mapper.Map<GetSaleItemResponse>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving sale item with ID: {SaleItemId}", id);
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<List<GetSaleItemResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSaleItems(CancellationToken cancellationToken,
                                                         [FromQuery] int page = 1,
                                                         [FromQuery] int size = 10)
        {
            if (page <= 0 || size <= 0)
            {
                _logger.LogWarning("Invalid pagination parameters: page={Page}, size={Size}", page, size);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Page number and size must be greater than zero."
                });
            }

            var command = new GetAllSaleItemsQuery
            {
                Skip = (page - 1) * size,
                Take = size
            };

            try
            {
                var response = await _mediator.Send(command, cancellationToken);
                var result = _mapper.Map<List<GetSaleItemResponse>>(response);

                _logger.LogInformation("Sale items retrieved with pagination: page={Page}, size={Size}", page, size);

                return Ok(new ApiResponseWithData<List<GetSaleItemResponse>>
                {
                    Success = true,
                    Message = "Sale items retrieved successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all sale items");
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSaleItem(Guid id,
                                                        [FromBody] UpdateSaleItemRequest request,
                                                        CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to update sale item with ID: {SaleItemId}", id);

            var validator = new UpdateSaleItemRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for sale item ID: {SaleItemId}. Errors: {Errors}", id, validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var command = _mapper.Map<UpdateSaleItemCommand>(request);
                command.SaleItemId = id;
                var response = await _mediator.Send(command, cancellationToken);

                if (response == null)
                {
                    _logger.LogWarning("Sale item with ID: {SaleItemId} not found", id);
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = $"Sale item with ID {id} not found"
                    });
                }

                _logger.LogInformation("Sale item with ID: {SaleItemId} updated successfully", id);

                return Ok(new ApiResponseWithData<UpdateSaleItemResponse>
                {
                    Success = true,
                    Message = "Sale item updated successfully",
                    Data = _mapper.Map<UpdateSaleItemResponse>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating sale item with ID: {SaleItemId}", id);
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSaleItem(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to delete sale item with ID: {SaleItemId}", id);

            var request = new DeleteSaleItemRequest { Id = id };
            var validator = new DeleteSaleItemRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for sale item ID: {SaleItemId}. Errors: {Errors}", id, validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var command = _mapper.Map<DeleteSaleItemCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                if (response == null)
                {
                    _logger.LogWarning("Sale item with ID: {SaleItemId} not found", id);
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = $"Sale item with ID {id} not found"
                    });
                }

                _logger.LogInformation("Sale item with ID: {SaleItemId} deleted successfully", id);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Sale item deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting sale item with ID: {SaleItemId}", id);
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
