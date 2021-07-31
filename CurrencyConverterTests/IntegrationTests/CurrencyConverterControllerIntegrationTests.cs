using CurrencyConverterAPI.Models;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverterUnitTests.IntegrationTests
{
    public class CurrencyConverterControllerIntegrationTests : IntegrationTest
    {
        [Fact]
        public async Task GetAllTransactionsByUserId_WithoutAnyPost_ReturnsEmptyResponse()
        {
            var response = await TestClient.GetAsync("api/v1/currencyconverter/2");

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().BeOfType<HttpResponseMessage>();
        }

        [Fact]
        public async Task GetAllTransactionsByUserId_WithPostAndGet_ReturnsSuccessResponse()
        {
            TransactionCreateInputParams transaction = new TransactionCreateInputParams();
            transaction.ClientId = 1;
            transaction.CurrencyTo = "BRL";
            transaction.CurrencyFrom = "EUR";
            transaction.Value = 10;

            await TestClient.PostAsJsonAsync("api/v1/CurrencyConverter", transaction);
            await TestClient.PostAsJsonAsync("api/v1/CurrencyConverter", transaction);
            var response = await TestClient.GetAsync("api/v1/currencyconverter/1");


            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().BeOfType<HttpResponseMessage>();
        }

        [Fact]
        public async Task InsertConversionTransaction_WithValidTransaction_ReturnsSuccessResponse()
        {
            TransactionCreateInputParams transaction = new TransactionCreateInputParams();
            transaction.ClientId = 1;
            transaction.CurrencyTo = "BRL";
            transaction.CurrencyFrom = "EUR";
            transaction.Value = 10;

            var response = await TestClient.PostAsJsonAsync("api/v1/currencyconverter", transaction);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Should().BeOfType<HttpResponseMessage>();
        }

        [Fact]
        public async Task InsertConversionTransaction_WithValidTransaction_ReturnsErrorIdUser()
        {
            TransactionCreateInputParams transaction = new TransactionCreateInputParams();
            transaction.ClientId = 0;
            transaction.CurrencyTo = "BRL";
            transaction.CurrencyFrom = "EUR";
            transaction.Value = 10;

            var response = await TestClient.PostAsJsonAsync("api/v1/currencyconverter", transaction);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().BeOfType<HttpResponseMessage>();
        }

        [Fact]
        public async Task InsertConversionTransaction_WithValidTransaction_ReturnsErrorCurrency()
        {
            TransactionCreateInputParams transaction = new TransactionCreateInputParams();
            transaction.ClientId = 1;
            transaction.CurrencyTo = "ASD";
            transaction.CurrencyFrom = "EUR";
            transaction.Value = 10;

            var response = await TestClient.PostAsJsonAsync("api/v1/currencyconverter", transaction);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().BeOfType<HttpResponseMessage>();
        }
    }
}
