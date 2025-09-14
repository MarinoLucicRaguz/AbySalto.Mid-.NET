using AbySalto.Mid.Application.Common;
using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.Infrastructure.External.DummyJson;
using AbySalto.Mid.Infrastructure.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AbySalto.Mid.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductApi _api;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ProductService> _logger;
        private readonly DummyJsonOptions _opt;

        public ProductService(IProductApi api, IMemoryCache cache, ILogger<ProductService> logger, IOptions<DummyJsonOptions> opt)
        {
            _api = api;
            _cache = cache;
            _logger = logger;
            _opt = opt.Value;
        }

        public Task<ServiceResponse<List<ProductDto>>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<PagedResult<ProductDto>>> GetAllPaginatedAsync(ProductQuery query, CancellationToken ct = default)
        {
            try
            {
                var limit = Math.Clamp(query.Size, 1, 100);
                var skip = Math.Max((query.Page - 1) * limit, 0);

                string cacheKey = $"products:{skip}:{limit}:{query.SortBy}:{query.Order}";
                if (_opt.CacheSeconds > 0 && _cache.TryGetValue(cacheKey, out PagedResult<ProductDto>? cached))
                    return ServiceResponse<PagedResult<ProductDto>>.Ok(cached);

                var envelope = await _api.GetProductsAsync(skip, limit, query.SortBy, query.Order, ct);
                var items = envelope.products.Select(Map).ToList();

                var result = new PagedResult<ProductDto>(items, envelope.total, envelope.skip, envelope.limit);

                if (_opt.CacheSeconds > 0)
                    _cache.Set(cacheKey, result, TimeSpan.FromSeconds(_opt.CacheSeconds));

                return ServiceResponse<PagedResult<ProductDto>>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch paginated products (page={Page}, size={Size})", query.Page, query.Size);
                return ServiceResponse<PagedResult<ProductDto>>.Fail("Unable to fetch products.");
            }
        }

        public async Task<ServiceResponse<ProductDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                var cacheKey = $"product:{id}";
                if (_opt.CacheSeconds > 0 && _cache.TryGetValue(cacheKey, out ProductDto? cached))
                    return ServiceResponse<ProductDto>.Ok(cached);

                var raw = await _api.GetProductByIdAsync(id, ct);
                var dto = Map(raw);

                if (_opt.CacheSeconds > 0)
                    _cache.Set(cacheKey, dto, TimeSpan.FromSeconds(_opt.CacheSeconds));

                return ServiceResponse<ProductDto>.Ok(dto);
            }
            catch (KeyNotFoundException)
            {
                return ServiceResponse<ProductDto>.Fail($"Product {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch product {ProductId}", id);
                return ServiceResponse<ProductDto>.Fail("Unable to fetch product.");
            }
        }

        private static ProductDto Map(ProductApiModel? p)
        {
            if (p is null) throw new ArgumentNullException(nameof(p));

            return new ProductDto(p.id, p.title, p.description, p.price, p.rating, p.stock);
        }
    }
}
