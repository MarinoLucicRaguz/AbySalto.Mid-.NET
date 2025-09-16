using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Mid.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route(nameof(GetAllPaginated))]
        public async Task<ActionResult<PagedResult<ProductDto>>> GetAllPaginated([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string? sortBy = null, [FromQuery] string? order = null, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (size < 1) size = 10;

            var response = await _productService.GetAllPaginatedAsync(new ProductQuery(page, size, sortBy, order), ct);
            return HandleResponse(response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken ct)
        {
            var response = await _productService.GetByIdAsync(id, ct);
            return HandleResponse(response);
        }
        
        [HttpGet]
        [Route(nameof(GetDetailsById) + "/{id:int}")]
        public async Task<ActionResult<ProductDetailDto>> GetDetailsById(int id, CancellationToken ct)
        {
            var response = await _productService.GetDetailsByIdAsync(id, ct);
            return HandleResponse(response);
        }
    }
}
