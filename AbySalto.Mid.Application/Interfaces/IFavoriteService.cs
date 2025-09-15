using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;

namespace AbySalto.Mid.Application.Interfaces
{
    public interface IFavoriteService
    {
        Task<ServiceResponse<FavoriteDto>> AddAsync(int userId, int productId, CancellationToken ct = default);
        Task<ServiceResponse<bool>> RemoveAsync(int userId, int productId, CancellationToken ct = default);
        Task<ServiceResponse<List<FavoriteDto>>> GetAllByUserAsync(int userId, CancellationToken ct = default);
    }
}
