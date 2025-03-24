using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<SalesController> _logger;

        public SalesController(IMediator mediator,
                               IMapper mapper,
                               ILogger<SalesController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger; 
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<List<GetSaleResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllSales(CancellationToken cancellationToken,
                                               [FromQuery] int? _page = null,
                                               [FromQuery] int? _size = null,
                                               [FromQuery] string sortBy = "saleDate",
                                               [FromQuery] string sortOrder = "asc")
        {
            int page = _page ?? 1;
            int size = _size ?? 10;

            if (page <= 0 || size <= 0)
            {
                _logger.LogWarning("Invalid pagination parameters: page={Page}, size={Size}", page, size);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Page number and size must be greater than zero."
                });
            }

            if (!new[] { "asc", "desc" }.Contains(sortOrder.ToLower()))
            {
                _logger.LogWarning("Invalid sortOrder: {SortOrder}", sortOrder);
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid sortOrder. Valid values are 'asc' or 'desc'."
                });
            }

            bool isAscending = sortOrder.ToLower() == "asc";

            var commandWithPaginationAndSorting = new GetAllSalesQuery
            {
                Skip = (page - 1) * size,
                Take = size,
                SortBy = sortBy.ToLower(),
                SortOrder = isAscending
            };

            try
            {
                var responsePaged = await _mediator.Send(commandWithPaginationAndSorting, cancellationToken);
                var resultPaged = _mapper.Map<List<GetSaleResponse>>(responsePaged);

                _logger.LogInformation("Sales retrieved with pagination: page={Page}, size={Size}, sortBy={SortBy}, sortOrder={SortOrder}", page, size, sortBy, sortOrder);

                return Ok(new ApiResponseWithData<List<GetSaleResponse>>
                {
                    Success = true,
                    Message = "Sales retrieved successfully",
                    Data = resultPaged
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving sales");
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSale(Guid id,
                                               CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to get sale with ID: {SaleId}", id);

            var request = new GetSalesQueryRequest { Id = id };

            var validator = new GetSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for sale ID: {SaleId}. Errors: {Errors}", id, validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var command = _mapper.Map<GetSaleCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                if (response == null)
                {
                    _logger.LogWarning("Sale with ID: {SaleId} not found", id);
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = $"Sale with ID {id} not found"
                    });
                }

                _logger.LogInformation("Sale with ID: {SaleId} retrieved successfully", id);
                return Ok(new ApiResponseWithData<GetSaleResponse>
                {
                    Success = true,
                    Message = "Sale retrieved successfully",
                    Data = _mapper.Map<GetSaleResponse>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving sale with ID: {SaleId}", id);
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request,
                                             CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to create sale with data: {SaleRequest}", request);

            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for sale request. Errors: {Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var command = _mapper.Map<CreateSaleCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Sale created successfully with ID: {SaleId}", response.Id);

                return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
                {
                    Success = true,
                    Message = "Sale created successfully",
                    Data = _mapper.Map<CreateSaleResponse>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating sale with data: {SaleRequest}", request);
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSale(Guid id,
                                                    CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to delete sale with ID: {SaleId}", id);

            var request = new DeleteSaleRequest { Id = id };
            var validator = new DeleteSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for sale ID: {SaleId}. Errors: {Errors}", id, validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var command = _mapper.Map<DeleteSaleCommand>(request);
                await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Sale with ID: {SaleId} deleted successfully", id);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Sale deleted successfully",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting sale with ID: {SaleId}", id);
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSale(Guid id,
                                                    [FromBody] UpdateSaleRequest request,
                                                    CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received request to update sale with ID: {SaleId}", id);

            var validator = new UpdateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for sale ID: {SaleId}. Errors: {Errors}", id, validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var command = _mapper.Map<UpdateSaleCommand>(request);
                command.SaleId = id;

                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Sale with ID: {SaleId} updated successfully", id);

                return Ok(new ApiResponseWithData<UpdateSaleResponse>
                {
                    Success = true,
                    Message = "Sale updated successfully",
                    Data = _mapper.Map<UpdateSaleResponse>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating sale with ID: {SaleId}", id);
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

    }
}

