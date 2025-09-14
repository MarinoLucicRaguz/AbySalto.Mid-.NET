using System.ComponentModel.DataAnnotations;

namespace AbySalto.Mid.Infrastructure.Options
{
    public class DummyJsonOptions
    {
        [Required, Url]
        public string BaseUrl { get; init; } = "https://dummyjson.com/";

        [Range(1, 60)]
        public int TimeoutSeconds { get; init; } = 10;

        [Range(0, 86400)]
        public int CacheSeconds { get; init; } = 60;
    }
}
