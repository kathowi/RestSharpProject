using Newtonsoft.Json;

namespace RestSharpProject
{
    public class Address
    {
        [JsonProperty("Street")]
        public string Street { get; set; }
        [JsonProperty("PostalCode")]
        public string PostalCode { get; set; }
        [JsonProperty("City")]
        public string City { get; set; }
        [JsonProperty("Country")]
        public string Country { get; set; }
        [JsonProperty("Id")]
        public int Id { get; set; }
    }
}
