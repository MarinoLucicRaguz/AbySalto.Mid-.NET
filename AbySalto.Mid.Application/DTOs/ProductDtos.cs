namespace AbySalto.Mid.Application.DTOs
{
    public record ProductDto(int Id, string Title, string Description, double Price, double Rating, int Stock);
    public record ProductQuery(int Page = 1, int Size = 20, string? SortBy = null, string? Order = null);
    public record ProductDetailExtendedDto(int Id, string Title, string Description, string Category, string Brand, string Sku, double Price, double DiscountPercentage, double Rating,
                                    int Stock, string AvailabilityStatus, int MinimumOrderQuantity, string WarrantyInformation, string ShippingInformation, string ReturnPolicy,
                                    double Weight, DimensionsDto Dimensions, IReadOnlyList<string> Tags, IReadOnlyList<string> Images, string Thumbnail, IReadOnlyList<ReviewDto> Reviews,
                                    MetaDto Meta);
    public record ProductDetailDto(int Id, string Title, string Description, string Category, string Brand, string Sku, double Price, double DiscountPercentage, double Rating,
                                    int Stock, string AvailabilityStatus, int MinimumOrderQuantity, string WarrantyInformation, string ShippingInformation, string ReturnPolicy, double Weight);
    public record DimensionsDto(double Width, double Height, double Depth);
    public record ReviewDto(double Rating, string Comment, DateTime Date, string ReviewerName, string ReviewerEmail);
    public record MetaDto(DateTime CreatedAt, DateTime UpdatedAt, string Barcode, string QrCode);

}
