using AbySalto.Mid.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AbySalto.Mid.WebApi.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult<T> HandleResponse<T>(ServiceResponse<T> response)
        {
            if (response == null) return StatusCode(500, "Null response");

            return StatusCode(response.StatusCode, response);
        }

        protected ActionResult<TResult> HandleResponse<T, TResult>(ServiceResponse<T> response, Func<T, TResult> transform)
        {
            if (response == null) return StatusCode(500, "Null response");
            if (!response.Success) return StatusCode(response.StatusCode, response);

            var newData = transform(response.Data!);
            var shaped = ServiceResponse<TResult>.Ok(newData, response.Message, response.StatusCode);
            return Ok(shaped);
        }

        protected int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User isnt logged in.");
            }

            return int.Parse(userIdClaim.Value);
        }
    }
}
