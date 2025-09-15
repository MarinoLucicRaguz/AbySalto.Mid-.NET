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
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<BasketDto>> Get(CancellationToken ct)
        {
            var response = await _basketService.GetBasketAsync(GetUserId(),ct);
            return HandleResponse(response);
        }

        [HttpPost]
        [Route(nameof(Add) + "/{productId:int}")]
        public async Task<ActionResult<BasketDto>> Add(int productId, [FromQuery] int increment = 1, CancellationToken ct = default)
        {
            var response = await _basketService.AddAsync(GetUserId(), productId, increment, ct);
            return HandleResponse(response);
        }

        [HttpPost]
        [Route(nameof(Reduce) + "/{productId:int}")]
        public async Task<ActionResult<BasketDto>> Reduce(int productId, [FromQuery] int decrement = 1, CancellationToken ct = default)
        {
            var resp = await _basketService.ReduceAsync(GetUserId(), productId, decrement, ct);
            return HandleResponse(resp);
        }

        [HttpDelete]
        [Route("{productId:int}")]
        public async Task<ActionResult<bool>> Remove(int productId, CancellationToken ct)
        {
            var resp = await _basketService.RemoveAsync(GetUserId(), productId, ct);
            return HandleResponse(resp);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> Clear(CancellationToken ct)
        {
            var resp = await _basketService.ClearAsync(GetUserId(), ct);
            return HandleResponse(resp);
        }
    }
}
