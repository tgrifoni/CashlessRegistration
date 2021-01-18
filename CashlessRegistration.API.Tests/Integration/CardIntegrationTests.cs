using CashlessRegistration.API.Models.DTOs.Card;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CashlessRegistration.API.Tests.Integration
{
    public class CardIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string ApplicationJson = "application/json";

        private readonly HttpClient _client;

        public CardIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory
                .WithWebHostBuilder(builder => builder
                    .UseEnvironment("Integration")
                    .ConfigureTestServices(services => services
                        .AddAuthentication(TestAuthHandler.AuthenticationScheme)
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { }))
                    .ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.json")))
                .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [Fact]
        public async Task Save_WhenRequestIsValid_ShouldSaveAndReturnProperResponse()
        {
            var validCustomerId = 1;
            var validCardNumber = 1234_5678_9012_3456;
            var validCvv = 405;
            var request = new SaveCardRequest(customerId: validCustomerId, cardNumber: validCardNumber, cvv: validCvv);

            var httpResponse = await _client.PostAsync("/api/card", GetStringContent(request));
            var json = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<SaveCardResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            httpResponse.EnsureSuccessStatusCode();
            Assert.NotNull(response);
            Assert.IsType<SaveCardResponse>(response);
            Assert.Equal(DateTime.UtcNow.Date, response.RegistrationDate.Date);
            Assert.NotEqual(default, response.CardId);
            Assert.NotEqual(default, response.Token);
        }

        [Fact]
        public async Task Validate_WhenRequestIsValid_ShouldValidateAndReturnProperResponse()
        {
            var validCustomerId = 1;
            var validCardId = 1;
            var validToken = 6345U;
            var validCvv = 405;
            var request = new ValidateCardRequest(customerId: validCustomerId, cardId: validCardId, token: validToken, cvv: validCvv);

            var response = await _client.PostAsync("/api/card/validate", GetStringContent(request));
            var responseString = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains("validated", responseString);
        }

        private StringContent GetStringContent<T>(T request) =>
            new StringContent(JsonSerializer.Serialize(request),
                Encoding.UTF8,
                ApplicationJson);
    }
}
