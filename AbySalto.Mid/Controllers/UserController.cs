using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Mid.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route(nameof(Register))]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequest registerRequest)
        {
            var response = await _userService.RegisterAsync(registerRequest);
            return HandleResponse(response);
        }

        [HttpPost]
        [Route(nameof(Login))]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginRequest loginRequest)
        {
            var response = await _userService.LoginAsync(loginRequest);
            return HandleResponse(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserData(CancellationToken ct = default)
        {
            var response = await _userService.GetUserAsync(GetUserId(), ct);
            return HandleResponse(response);
        }
    }
}
