using FluentAssertions;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;

namespace RestSharpProject
{
    [TestFixture]
    public class DummyTests
    {
        [Test]
        public async Task CheckGetAddressessTest()
        {
            int successCode = 200;
            var client = new RestClient("https://objwsw.azurewebsites.net/api/addresses");
            client.Authenticator = new HttpBasicAuthenticator("testuser", "testuser");

            var request = new RestRequest();
            var response = await client.GetAsync(request);
            var statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;

            numericStatusCode.Should().Be(successCode);
        }

        [Test]
        [TestCase(1)]
        public async Task CheckGetAddressByIdTest(int id)
        {
            string expectedStreet = "4649 Peachwillow";
            string expectedCity = "Test";
            string expectedPostalCode = "20-653";
            string expectedCountry = "Test";
            var client = new RestClient("https://objwsw.azurewebsites.net/api/addresses");
            client.Authenticator = new HttpBasicAuthenticator("testuser", "testuser");

            var request = new RestRequest($"/{id}");
            var response = await client.GetAsync(request);
            var responseBody = response.Content;
            Address address = JsonSerializer.Deserialize<Address>(responseBody);

            address.Street.Should().Be(expectedStreet);
            address.PostalCode.Should().Be(expectedPostalCode);
            address.City.Should().Be(expectedCity);
            address.Country.Should().Be(expectedCountry);
        }

        [Test]
        public async Task CheckPostAddressTest()
        {
            int successCode = 201;
            string street = "Kwiatowa";
            string postalCode = "20-653";
            string city = "Wroclaw";
            string country = "Poland";

            var address = new Address
            {
                Street = street,
                PostalCode = postalCode,
                City = city,
                Country = country
            };

            var client = new RestClient("https://objwsw.azurewebsites.net/api/addresses");
            client.Authenticator = new HttpBasicAuthenticator("testuser", "testuser");

            var request = new RestRequest().AddJsonBody(address);
            var response = await client.PostAsync(request);
            var responseBody = response.Content;
            Address actualAddress = JsonSerializer.Deserialize<Address>(responseBody);
            var statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;

            numericStatusCode.Should().Be(successCode);
            numericStatusCode.Should().Be(successCode);
            actualAddress.Street.Should().Be(street);
            actualAddress.PostalCode.Should().Be(postalCode);
            actualAddress.City.Should().Be(city);
            actualAddress.Country.Should().Be(country);
        }

        [Test]
        public async Task CheckUpdateAddressTest()
        {
            int successCode = 200;
            var address = new Address
            {
                Street = "Kwiatowa",
                PostalCode = "20-600",
                City = "Wroclaw",
                Country = "Poland"
            };

            var client = new RestClient("https://objwsw.azurewebsites.net/api/addresses");
            client.Authenticator = new HttpBasicAuthenticator("testuser", "testuser");
            var request = new RestRequest().AddJsonBody(address);
            var responsePost = await client.PostAsync(request);
            var responseBodyForAddedAddress = responsePost.Content;
            Address addedAddress = JsonSerializer.Deserialize<Address>(responseBodyForAddedAddress);
            var id = addedAddress.Id;

            string street = "Owocowa";
            string postalCode = "22-610";
            string city = "Kraków";
            string country = "Poland";

            var newAddress = new Address
            {
                Id = id,
                Street = street,
                PostalCode = postalCode,
                City = city,
                Country = country
            };

            var updateRequest = new RestRequest($"/{id}").AddJsonBody(newAddress);
            var responsePut = await client.PutAsync(updateRequest);

            var responseBodyForUpdatedAddress = responsePut.Content;
            Address actualAddress = JsonSerializer.Deserialize<Address>(responseBodyForUpdatedAddress);

            var statusCode = responsePut.StatusCode;
            int numericStatusCode = (int)statusCode;

            numericStatusCode.Should().Be(successCode);
            numericStatusCode.Should().Be(successCode);
            actualAddress.Street.Should().Be(street);
            actualAddress.PostalCode.Should().Be(postalCode);
            actualAddress.City.Should().Be(city);
            actualAddress.Country.Should().Be(country);
        }

        [Test]
        public async Task CheckDeleteAddressTest()
        {
            var client = new RestClient("https://objwsw.azurewebsites.net/api/addresses");
            client.Authenticator = new HttpBasicAuthenticator("testuser", "testuser");

            var address = new Address
            {
                Street = "Kwiatowa1",
                PostalCode = "20-653",
                City = "Wroclaw",
                Country = "Poland"
            };

            int successCode = 200;

            var request = new RestRequest().AddJsonBody(address);
            var response = await client.PostAsync(request);

            var responseBody = response.Content;
            Address addedAddress = JsonSerializer.Deserialize<Address>(responseBody);

            var id = addedAddress.Id;

            var deleteRequest = new RestRequest($"/{id}");
            var responseStatus = await client.DeleteAsync(deleteRequest);

            var statusCode = responseStatus.StatusCode;
            int numericStatusCode = (int)statusCode;

            numericStatusCode.Should().Be(successCode);
        }

        [Test]
        public async Task CheckErrorMessageWhenAddressIsNotFoundTest()
        {
            var client = new RestClient("https://objwsw.azurewebsites.net/api/addresses");
            client.Authenticator = new HttpBasicAuthenticator("testuser", "testuser");

            int notFoundCode = 404;
            string expectedMessage = "Request failed with status code NotFound";

            try
            {
                var deleteRequest = new RestRequest($"/{0}");
                var response = await client.DeleteAsync(deleteRequest);
            }
            catch (HttpRequestException ex)
            {
                var statusCode = ex.StatusCode;
                int numericStatusCode = (int)statusCode;
                var errorMessage = ex.Message;

                numericStatusCode.Should().Be(notFoundCode);
                errorMessage.Should().Be(expectedMessage); 
            }
        }
    }
};