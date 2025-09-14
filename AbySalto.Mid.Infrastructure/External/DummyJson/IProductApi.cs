namespace AbySalto.Mid.Infrastructure.External.DummyJson
{
    public interface IProductApi
    {
        Task<ProductsEnvelope> GetProductsAsync(int skip, int limit, string? sortBy = null, string? order = null, CancellationToken ct = default);
        Task<ProductApiModel?> GetProductByIdAsync(int id, CancellationToken ct = default);
    }
}
