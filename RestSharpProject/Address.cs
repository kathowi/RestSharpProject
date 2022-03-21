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

        public Address(string street, string postalCode, string city, string country)
        {
            Street = street;
            PostalCode = postalCode;
            City = city;                
            Country = country;
        }

        public Address(int id, string street, string postalCode, string city, string country)
        {
            Id = id;
            Street = street;
            PostalCode = postalCode;
            City = city;
            Country = country;
        }
    }
}
