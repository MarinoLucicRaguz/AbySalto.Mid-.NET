using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Mapper
{
    public static class FavoriteMapper
    {
        public static FavoriteDto ToDto(Favorite favorite, ProductDto? product = null)
        {
            return new FavoriteDto(favorite.Id, favorite.ProductId, product);
        }  
        
        public static FavoriteBasicDto ToDto(Favorite favorite)
        {
            return new FavoriteBasicDto(favorite.Id, favorite.ProductId);
        }
    }
}
