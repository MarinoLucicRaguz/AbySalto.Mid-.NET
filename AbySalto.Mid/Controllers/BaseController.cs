using AbySalto.Mid.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Mid.WebApi.Controllers
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        protected IActionResult HandleResponse<T>(ServiceResponse<T> response)
        {
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
