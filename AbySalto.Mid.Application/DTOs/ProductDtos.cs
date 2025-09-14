namespace AbySalto.Mid.Application.DTOs
{
    public record ProductDto(int Id, string Title, string Description, double Price, double Rating, int Stock);
    public record ProductQuery(int Page = 1, int Size = 20, string? SortBy = null, string? Order = null);
}
