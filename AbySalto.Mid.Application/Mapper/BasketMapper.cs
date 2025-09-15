using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Mapper
{
    public class BasketMapper
    {
        public static BasketItemDto ToItemDto(Basket item, ProductDto? product = null)
        {
            return new BasketItemDto(item.ProductId, item.Quantity, product);
        }

        public static BasketDto ToDto(IEnumerable<BasketItemDto> items)
        {
            var list = items.ToList();

            return new BasketDto(list.Count, list.Sum(i => i.Quantity), list.Sum(i => (i.Product?.Price ?? 0) * i.Quantity), list);
        }
    }
}
