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
        RestClient client = new RestClient("https://objwsw.azurewebsites.net/api/addresses");
        int successCode200 = 200;
        int successCode201 = 201;
        int notFoundCode404 = 404; //use httstatus code, system.net
        string expectedMessage = "Request failed with status code NotFound";

        [SetUp]
        public void BeforeEach()
        {
            client.Authenticator = new HttpBasicAuthenticator("testuser", "testuser");
        }

        [Test]
        public async Task CheckGetAddressessTest()
        {
            //arrange
            var request = new RestRequest();
            var response = await client.GetAsync(request);

            //act
            var statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;

            //assert
            numericStatusCode.Should().Be(successCode200);
        }

        [Test, TestCaseSource(typeof(TestData), "AddressGetTestData")]

        public async Task CheckGetAddressByIdTest(int id, string street, string postalCode, string city, string country)
        {
            //arrange
            var request = new RestRequest($"/{id}");
            var response = await client.GetAsync(request);

            //act
            var responseBody = response.Content;
            Address address = JsonSerializer.Deserialize<Address>(responseBody);

            //assert
            address.Street.Should().Be(street);
            address.PostalCode.Should().Be(postalCode);
            address.City.Should().Be(city);
            address.Country.Should().Be(country);
        }

        [Test, TestCaseSource(typeof(TestData), "AddressPostTestData")]

        public async Task CheckPostAddressTest(string street, string postalCode, string city, string country)
        {
            //arrange
            var address = new Address(street, postalCode, city, country);

            var request = new RestRequest().AddJsonBody(address);
            var response = await client.PostAsync(request);

            //act
            var responseBody = response.Content;
            Address actualAddress = JsonSerializer.Deserialize<Address>(responseBody);
            var statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;

            //assert
            numericStatusCode.Should().Be(successCode201);
            actualAddress.Street.Should().Be(street);
            actualAddress.PostalCode.Should().Be(postalCode);
            actualAddress.City.Should().Be(city);
            actualAddress.Country.Should().Be(country);
        }

        [Test, TestCaseSource(typeof(TestData), "AddressUpdateTestData")]

        public async Task CheckUpdateAddressTest
            (string street, string postalCode, string city, string country, string newStreet, string newPostalCode, string newCity, string newCountry)
        {
            //arrange
            var address = new Address(street, postalCode, city, country);

            var request = new RestRequest().AddJsonBody(address);
            var responsePost = await client.PostAsync(request);

            //act
            var responseBodyForAddedAddress = responsePost.Content;
            Address addedAddress = JsonSerializer.Deserialize<Address>(responseBodyForAddedAddress);
            var id = addedAddress.Id;

            var newAddress = new Address(id, newStreet, newPostalCode, newCity, newCountry);   
 

            var updateRequest = new RestRequest($"/{id}").AddJsonBody(newAddress);
            var responsePut = await client.PutAsync(updateRequest);

            //act
            var responseBodyForUpdatedAddress = responsePut.Content;
            Address actualAddress = JsonSerializer.Deserialize<Address>(responseBodyForUpdatedAddress);
            var statusCode = responsePut.StatusCode;
            int numericStatusCode = (int)statusCode;

            //assert
            numericStatusCode.Should().Be(successCode200);
            actualAddress.Street.Should().Be(newStreet);
            actualAddress.PostalCode.Should().Be(newPostalCode);
            actualAddress.City.Should().Be(newCity);
            actualAddress.Country.Should().Be(newCountry);
        }

        [Test, TestCaseSource(typeof(TestData), "AddressPostTestData")]

        public async Task CheckDeleteAddressTest(string street, string postalCode, string city, string country)
        {
            //arrange
            var address = new Address(street, postalCode, city, country);

            var request = new RestRequest().AddJsonBody(address);
            var response = await client.PostAsync(request);

            //act
            var responseBody = response.Content;
            Address addedAddress = JsonSerializer.Deserialize<Address>(responseBody);
            var id = addedAddress.Id;
            var deleteRequest = new RestRequest($"/{id}");
            var responseStatus = await client.DeleteAsync(deleteRequest);
            var statusCode = responseStatus.StatusCode;
            int numericStatusCode = (int)statusCode;

            //assert
            numericStatusCode.Should().Be(successCode200);
        }

        [Test]

        public async Task CheckErrorMessageWhenAddressIsNotFoundTest()
        {
            //arrange
            try
            {
                var deleteRequest = new RestRequest($"/{0}");
                var response = await client.DeleteAsync(deleteRequest);
            }
            //act
            catch (HttpRequestException ex)
            {
                var statusCode = ex.StatusCode;
                int numericStatusCode = (int)statusCode;
                var errorMessage = ex.Message;

             //assert
                numericStatusCode.Should().Be(notFoundCode404);
                errorMessage.Should().Be(expectedMessage); 
            }
        }
    }
};