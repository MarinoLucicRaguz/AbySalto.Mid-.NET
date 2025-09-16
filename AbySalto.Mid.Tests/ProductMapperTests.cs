using AbySalto.Mid.Infrastructure.External.DummyJson;

namespace AbySalto.Mid.Tests;

public class ProductMapperTests
{
    [Fact]
    public void ToDto_MapsCorrectly()
    {
        var apiModel = new ProductApiModel
        {
            id = 1,
            title = "Test Product",
            description = "A product for testing",
            category = "Test Category",
            brand = "Test Brand",
            sku = "SKU123",
            price = 99.99,
            discountPercentage = 10,
            rating = 4.5,
            stock = 10,
            weight = 2,
            warrantyInformation = "1 year warranty",
            shippingInformation = "Ships in 2-3 days",
            availabilityStatus = "In Stock",
            returnPolicy = "30 days return",
            minimumOrderQuantity = 1
        };


        var dto = ProductMapper.ToDetailDto(apiModel);

        Assert.Equal(apiModel.id, dto.Id);
        Assert.Equal(apiModel.title, dto.Title);
        Assert.Equal(apiModel.description, dto.Description);
        Assert.Equal(apiModel.price, dto.Price);
        Assert.Equal(apiModel.rating, dto.Rating);
        Assert.Equal(apiModel.stock, dto.Stock);
        Assert.Equal(apiModel.category, dto.Category);
        Assert.Equal(apiModel.brand, dto.Brand);
        Assert.Equal(apiModel.sku, dto.Sku);
        Assert.Equal(apiModel.discountPercentage, dto.DiscountPercentage);
        Assert.Equal(apiModel.availabilityStatus, dto.AvailabilityStatus);
    }
}
