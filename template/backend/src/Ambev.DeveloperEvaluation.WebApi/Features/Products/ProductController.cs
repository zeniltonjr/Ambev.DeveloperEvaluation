using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdatePudct;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMediator mediator,
                                 IMapper mapper,
                                 ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<List<GetProductResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken, [FromQuery] int? _page = null, [FromQuery] int? _size = null)
        {
            try
            {
                int page = _page ?? 1;
                int size = _size ?? 10;

                if (page <= 0 || size <= 0)
                {
                    _logger.LogWarning("Invalid page or size parameters. Page: {Page}, Size: {Size}", page, size);
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Page number and size must be greater than zero."
                    });
                }

                var commandWithPagination = new GetAllProductsQuery
                {
                    Skip = (page - 1) * size,
                    Take = size
                };

                var responsePaged = await _mediator.Send(commandWithPagination, cancellationToken);
                var resultPaged = _mapper.Map<List<GetProductResponse>>(responsePaged);

                _logger.LogInformation("Successfully retrieved {Count} products.", resultPaged.Count);

                return Ok(new ApiResponseWithData<List<GetProductResponse>>
                {
                    Success = true,
                    Message = "Products retrieved successfully",
                    Data = resultPaged
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting products.");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving the products."
                });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateProductRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Product creation failed due to validation errors: {Errors}", validationResult.Errors);
                    return BadRequest(validationResult.Errors);
                }

                var command = _mapper.Map<CreateProductCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Product created successfully with Id: {ProductId}", response.Id);

                return Created(string.Empty, new ApiResponseWithData<CreateProductResponse>
                {
                    Success = true,
                    Message = "Product created successfully",
                    Data = _mapper.Map<CreateProductResponse>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating product.");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while creating the product."
                });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new GetProductRequest { Id = id };
                var validator = new GetProductRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Product retrieval failed due to validation errors: {Errors}", validationResult.Errors);
                    return BadRequest(validationResult.Errors);
                }

                var command = _mapper.Map<GetProductCommand>(request.Id);
                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Successfully retrieved product with Id: {ProductId}", id);

                return Ok(new ApiResponseWithData<GetProductResponse>
                {
                    Success = true,
                    Message = "Product retrieved successfully",
                    Data = _mapper.Map<GetProductResponse>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting product with Id: {ProductId}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving the product."
                });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new DeleteProductRequest { Id = id };
                var validator = new DeleteProductRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Product deletion failed due to validation errors: {Errors}", validationResult.Errors);
                    return BadRequest(validationResult.Errors);
                }

                var command = _mapper.Map<DeleteProductCommand>(id);
                await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Product deleted successfully with Id: {ProductId}", id);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Product deleted successfully",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting product with Id: {ProductId}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while deleting the product."
                });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdatePuduct.UpdateProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UpdateProductRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Product update failed due to validation errors: {Errors}", validationResult.Errors);
                    return BadRequest(validationResult.Errors);
                }

                var command = _mapper.Map<UpdateProductCommand>(request);
                command.Id = id;

                var response = await _mediator.Send(command, cancellationToken);

                _logger.LogInformation("Product updated successfully with Id: {ProductId}", id);

                return Ok(new ApiResponseWithData<UpdatePuduct.UpdateProductResponse>
                {
                    Success = true,
                    Message = "Product updated successfully",
                    Data = _mapper.Map<UpdatePuduct.UpdateProductResponse>(response)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating product with Id: {ProductId}", id);
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while updating the product."
                });
            }
        }
    }
}
