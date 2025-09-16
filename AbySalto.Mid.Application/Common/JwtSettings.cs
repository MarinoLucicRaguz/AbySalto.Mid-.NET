using System.ComponentModel.DataAnnotations;

namespace AbySalto.Mid.Application.Common
{
    public class JwtSettings
    {
        [Required]
        public string Key { get; set; } = string.Empty;
        [Required]
        public string Issuer { get; set; } = string.Empty;
        [Required]
        public string Audience { get; set; } = string.Empty;
        [Range(1, 1440)]
        public int ExpireMinutes { get; set; } = 15;
        [Range(1, 30)]
        public int RefreshTokenDays { get; set; } = 7;
    }
}
