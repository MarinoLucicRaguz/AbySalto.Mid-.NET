using AbySalto.Mid.Application.DTOs;
using AbySalto.Mid.Domain.Entities;

namespace AbySalto.Mid.Application.Mapper
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            return new UserDto(user.Id, user.Username, user.Email, user.FirstName, user.LastName,
                 (user.BasketItems ?? new List<Basket>()).Select(b => BasketMapper.ToItemDto(b)).ToList(),
                 (user.Favorites ?? new List<Favorite>()).Select(f => FavoriteMapper.ToDto(f)).ToList()
            );
        }
    }
}
