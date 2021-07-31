using CurrencyConverterAPI;
using CurrencyConverterAPI.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http;

namespace CurrencyConverterUnitTests.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(ConvertTransactionContext));
                        services.AddDbContext<ConvertTransactionContext>(options => { options.UseInMemoryDatabase("testDB"); });
                    });
                });
            TestClient = appFactory.CreateClient();
        }
    }
}
