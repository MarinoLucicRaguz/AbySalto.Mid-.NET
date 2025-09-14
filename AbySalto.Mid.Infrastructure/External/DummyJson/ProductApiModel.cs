namespace AbySalto.Mid.Infrastructure.External.DummyJson
{
    public class ProductApiModel
    {
        public int id { get; set; }
        public string title { get; set; } = "";
        public string description { get; set; } = "";
        public string category { get; set; } = "";
        public double price { get; set; }
        public double rating { get; set; }
        public int stock { get; set; }
        public string brand { get; set; } = "";
        public string thumbnail { get; set; } = "";
    }
}
