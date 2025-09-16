using AbySalto.Mid.Application.Common;
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
            return HandleResponse(response, data =>
            {
                SetRefreshCookie(data.RefreshToken.Token, data.RefreshToken.Expires);
                return new AuthResponseDto(data.AccessToken, data.User);
            });
        }

        [HttpPost]
        [Route(nameof(Login))]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginRequest loginRequest)
        {
            var response = await _userService.LoginAsync(loginRequest);
            return HandleResponse(response, data =>
            {
                SetRefreshCookie(data.RefreshToken.Token, data.RefreshToken.Expires);
                return new AuthResponseDto(data.AccessToken, data.User);
            });
        }

        [HttpPost(nameof(Refresh))]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(ServiceResponse<AuthResponseDto>.Fail("No refresh token found.", 401));
            }

            var response = await _userService.RefreshAsync(refreshToken);
            return HandleResponse(response, data =>
            {
                SetRefreshCookie(data.RefreshToken.Token, data.RefreshToken.Expires);
                return new AuthResponseDto(data.AccessToken, data.User);
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserData(CancellationToken ct = default)
        {
            var response = await _userService.GetUserAsync(GetUserId(), ct);
            return HandleResponse(response);
        }

        [HttpPost]
        [Route(nameof(Logout))]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(token))
            {
                await _userService.RevokeAsync(token);
                Response.Cookies.Delete("refreshToken");
            }

            return Ok();
        }

        private void SetRefreshCookie(string token, DateTime expiresUtc)
        {
            Response.Cookies.Append("refreshToken", token, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict, Expires = expiresUtc });
        }
    }
}
