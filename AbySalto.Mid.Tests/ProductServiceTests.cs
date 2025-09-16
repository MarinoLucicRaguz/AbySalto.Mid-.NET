using AbySalto.Mid.Infrastructure.External.DummyJson;
using AbySalto.Mid.Infrastructure.Options;
using AbySalto.Mid.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace AbySalto.Mid.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_ShouldReturnCachedValueOnSecondCall()
        {
            var apiMock = new Mock<IProductApi>();
            apiMock.Setup(a => a.GetProductByIdAsync(1, default))
                   .ReturnsAsync(new ProductApiModelExtended
                   {
                       id = 1,
                       title = "Test",
                       description = "Dummy",
                       price = 10,
                       stock = 5,
                       rating = 4.5
                   });

            var cache = new MemoryCache(new MemoryCacheOptions());
            var opts = Options.Create(new DummyJsonOptions { CacheSeconds = 30 });
            var service = new ProductService(apiMock.Object, cache, NullLogger<ProductService>.Instance, opts);

            var first = await service.GetByIdAsync(1);
            var second = await service.GetByIdAsync(1);

            apiMock.Verify(a => a.GetProductByIdAsync(1, default), Times.Once);
            Assert.True(second.Success);
            Assert.Equal("Test", second.Data?.Title);
        }
    }
}
