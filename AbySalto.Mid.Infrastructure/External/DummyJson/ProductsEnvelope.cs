namespace AbySalto.Mid.Infrastructure.External.DummyJson
{
    public class ProductsEnvelope
    {
        public List<ProductApiModel> products { get; set; } = new List<ProductApiModel>();
        public int total { get; set; }
        public int skip { get; set; }
        public int limit { get; set; }
    }
}
