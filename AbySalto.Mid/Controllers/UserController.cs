using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Mid.WebApi.Controllers
{
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
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var response = await _userService.RegisterAsync(registerRequest);
            return HandleResponse(response);
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var response = await _userService.LoginAsync(loginRequest);
            return HandleResponse(response);
        }
    }
}
