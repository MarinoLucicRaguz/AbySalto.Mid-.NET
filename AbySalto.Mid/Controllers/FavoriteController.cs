using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Mid.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoriteController : BaseController
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost]
        [Route("{productId:int}")]
        public async Task<ActionResult<FavoriteDto>> Add(int productId, CancellationToken ct)
        {
            var result = await _favoriteService.AddAsync(GetUserId(), productId, ct);
            return HandleResponse(result);
        }

        [HttpDelete]
        [Route("{productId:int}")]
        public async Task<ActionResult<bool>> Remove(int productId, CancellationToken ct)
        {
            var result = await _favoriteService.RemoveAsync(GetUserId(), productId, ct);
            return HandleResponse(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<FavoriteDto>>> GetAll(CancellationToken ct)
        {
            var result = await _favoriteService.GetAllByUserAsync(GetUserId(), ct);
            return HandleResponse(result);
        }
    }
}
