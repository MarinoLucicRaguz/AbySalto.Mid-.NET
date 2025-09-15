using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;

namespace AbySalto.Mid.Application.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<AuthResponseDto>> RegisterAsync(RegisterRequest request); 
        Task<ServiceResponse<AuthResponseDto>> LoginAsync(LoginRequest request);
        Task<ServiceResponse<UserDto>> GetUserAsync(int id, CancellationToken ct = default);
    }
}
