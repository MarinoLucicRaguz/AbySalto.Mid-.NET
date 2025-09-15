namespace AbySalto.Mid.Application.DTOs
{
    public record BasketItemDto(int ProductId, int Quantity, ProductDto? Product);
    public record BasketDto(int TotalItems, int TotalQuantity, double TotalPrice, List<BasketItemDto> Items);
}
