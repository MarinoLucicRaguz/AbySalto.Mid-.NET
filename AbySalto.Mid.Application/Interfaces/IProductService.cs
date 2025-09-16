using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;

namespace AbySalto.Mid.Application.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<PagedResult<ProductDto>>> GetAllPaginatedAsync(ProductQuery query, CancellationToken ct = default);
        Task<ServiceResponse<ProductDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ServiceResponse<ProductDetailDto>> GetDetailsByIdAsync(int id, CancellationToken ct = default);
    }
}
