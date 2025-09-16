using AbySalto.Mid.Infrastructure.External.DummyJson;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace AbySalto.Mid.Infrastructure.External
{
    public class DummyJsonApiClient : IProductApi
    {
        private readonly HttpClient _client;
        private readonly ILogger<DummyJsonApiClient> _logger;
        private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

        public DummyJsonApiClient(HttpClient client, ILogger<DummyJsonApiClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<ProductsEnvelope> GetProductsAsync(int skip, int limit, string? sortBy = null, string? order = null, CancellationToken ct = default)
        {
            var baseFields = new List<string> { "id", "title", "description", "price", "rating", "stock" };
            if (!string.IsNullOrEmpty(sortBy) && !baseFields.Contains(sortBy.ToLower()))
            {
                baseFields.Add(sortBy);
            }

            var select = string.Join(",", baseFields);

            var url = $"products?limit={limit}&skip={skip}&select={select}";
            if (!string.IsNullOrEmpty(sortBy))
            {
                url = url + $"&sortBy={sortBy}";
            }

            if (!string.IsNullOrEmpty(order))
            {
                url = url + $"&order={order}";
            }

            using var response = await _client.GetAsync(url, ct);

            if (!response.IsSuccessStatusCode)
            {
                throw await CreateHttpException(response, $"GET {url}");
            }

            var body = await response.Content.ReadFromJsonAsync<ProductsEnvelope>(_jsonOptions, ct);
            return body ?? new ProductsEnvelope();
        }

        public async Task<ProductApiModelExtended?> GetProductByIdAsync(int id, CancellationToken ct = default)
        {
            var url = $"products/{id}";
            using var response = await _client.GetAsync(url, ct);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Product {id} not found");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw await CreateHttpException(response, $"GET {url}");
            }

            var body = await response.Content.ReadFromJsonAsync<ProductApiModelExtended>(_jsonOptions, ct);
            return body ?? throw new InvalidOperationException("Product payload was empty.");
        }

        private async Task<HttpRequestException> CreateHttpException(HttpResponseMessage response, string op)
        {
            string details = "";
            try
            {
                details = await response.Content.ReadAsStringAsync();
            }
            catch
            {
            }

            _logger.LogWarning("DummyJSON call failed ({Op}) with {Status} {Reason}. Body: {Body}", op, (int)response.StatusCode, response.ReasonPhrase, Truncate(details, 500));

            return new HttpRequestException($"DummyJSON failed: {op} {(int)response.StatusCode} {response.ReasonPhrase}");
        }

        private static string Truncate(string s, int max) => s.Length <= max ? s : s[..max];
    }
}
