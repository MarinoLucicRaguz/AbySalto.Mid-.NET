using AbySalto.Mid.Application.Common;
using Microsoft.AspNetCore.Mvc;

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
    }
}
