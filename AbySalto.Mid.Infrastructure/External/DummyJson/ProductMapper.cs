using AbySalto.Mid.Application.DTOs;

namespace AbySalto.Mid.Infrastructure.External.DummyJson
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(ProductApiModel? productApiModel)
        {
            if (productApiModel == null) throw new ArgumentNullException(nameof(productApiModel));

            return new ProductDto(productApiModel.id, productApiModel.title, productApiModel.description, productApiModel.price, productApiModel.rating, productApiModel.stock);
        }

        public static ProductDetailDto ToDetailDto(ProductApiModel? productApiModel)
        {
            if (productApiModel == null) throw new ArgumentNullException(nameof(productApiModel));

            return new ProductDetailDto(productApiModel.id, productApiModel.title, productApiModel.description, productApiModel.category, productApiModel.brand, productApiModel.sku, productApiModel.price, productApiModel.discountPercentage,
                                        productApiModel.rating, productApiModel.stock, productApiModel.availabilityStatus, productApiModel.minimumOrderQuantity, productApiModel.warrantyInformation, productApiModel.shippingInformation,
                                        productApiModel.returnPolicy, productApiModel.weight, new DimensionsDto(productApiModel.dimensions.width, productApiModel.dimensions.height, productApiModel.dimensions.depth), productApiModel.tags,
                                        productApiModel.images, productApiModel.thumbnail, productApiModel.reviews.Select(r => new ReviewDto(r.rating, r.comment, r.date, r.reviewerName, r.reviewerEmail)).ToList(),
                                        new MetaDto(productApiModel.meta.createdAt, productApiModel.meta.updatedAt, productApiModel.meta.barcode, productApiModel.meta.qrCode));
        }
    }
}

