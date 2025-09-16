using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;

namespace AbySalto.Mid.Application.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<PagedResult<ProductDetailDto>>> GetAllPaginatedAsync(ProductQuery query, CancellationToken ct = default);
        Task<ServiceResponse<ProductDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ServiceResponse<ProductDetailExtendedDto>> GetDetailsByIdAsync(int id, CancellationToken ct = default);
    }
}
