using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;

namespace AbySalto.Mid.Application.Interfaces
{
    public interface IBasketService
    {
        Task<ServiceResponse<BasketDto>> GetBasketAsync(int userId, CancellationToken ct = default);
        Task<ServiceResponse<BasketDto>> AddAsync(int userId, int productId, int incrementBy = 1, CancellationToken ct = default);
        Task<ServiceResponse<BasketDto>> ReduceAsync(int userId, int productId, int decrementBy = -1, CancellationToken ct = default);
        Task<ServiceResponse<bool>> RemoveAsync(int userId, int productId, CancellationToken ct = default);
        Task<ServiceResponse<bool>> ClearAsync(int userId, CancellationToken ct = default);
    }
}
