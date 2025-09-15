namespace AbySalto.Mid.Application.DTOs
{
    public record FavoriteDto(int Id, int ProductId, ProductDto? Product);
    public record FavoriteBasicDto(int Id, int ProductId);
}
