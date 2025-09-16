using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;

namespace AbySalto.Mid.Application.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<AuthResult>> RegisterAsync(RegisterRequest request); 
        Task<ServiceResponse<AuthResult>> LoginAsync(LoginRequest request);
        Task<ServiceResponse<UserDto>> GetUserAsync(int id, CancellationToken ct = default);
        Task<ServiceResponse<AuthResult>> RefreshAsync(string refreshToken);
        Task<ServiceResponse<bool>> RevokeAsync(string refreshToken);
    }
}
