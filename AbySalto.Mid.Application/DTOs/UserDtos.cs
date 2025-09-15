namespace AbySalto.Mid.Application.DTOs
{
    public record RegisterRequest(string Username, string Email, string Password);
    public record LoginRequest(string Email, string Password);
    public record UserDto(int Id, string Username, string Email, string? FirstName, string? LastName, List<BasketItemDto> BasketItems, List<FavoriteBasicDto> Favorites);
    public record AuthResponseDto(string Token, UserDto user);
}
