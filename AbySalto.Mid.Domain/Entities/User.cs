namespace AbySalto.Mid.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<Favorite>? Favorites { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}
