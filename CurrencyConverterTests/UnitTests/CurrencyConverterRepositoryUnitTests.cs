using CurrencyConverterAPI.Data;
using CurrencyConverterAPI.Models;
using CurrencyConverterAPI.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverterUnitTests.UnitTests
{
    public class CurrencyConverterRepositoryUnitTests
    {
        private static DbContextOptions<ConvertTransactionContext> DbContext()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<ConvertTransactionContext>()
                .UseInMemoryDatabase(databaseName: "Test").UseInternalServiceProvider(serviceProvider)
                .Options;

            return options;
        }

        private static IConfigurationRoot MyConfiguration()
        {
            var myConfiguration = new Dictionary<string, string>
                {
                    {"AppSettings:ACCESS_KEY", "be24a7b7fc131b54608f3874ee9af76e"},
                    {"AppSettings:BASE_URL", "http://api.exchangeratesapi.io/latest"}
                };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
            return configuration;
        }

        [Fact]
        public async Task insertConversionTransactionAsync_WithInsertTransacion_ReturnsTransactionOk()
        {
            TransactionCreateInputParams input = new TransactionCreateInputParams();
            input.ClientId = 5;
            input.CurrencyTo = "EUR";
            input.CurrencyFrom = "USD";
            input.Value = 8;

            TransactionCreateResult result;

            using (var context = new ConvertTransactionContext(DbContext()))
            {
                var repository = new CurrencyConverterRepository(context, MyConfiguration(), new Mock<ILogger<CurrencyConverterRepository>>().Object);

                result = await repository.insertConversionTransactionAsync(input);
            }

            var resultOk = result.Should().BeOfType<TransactionCreateResult>().Subject;

            resultOk.TransactionId.Should().NotBeEmpty();
            resultOk.ValueFrom.Should().BePositive();
        }

        [Fact]
        public async Task insertConversionTransactionAsync_WithNoValidCurrency_ReturnsInvalidCurrency()
        {
            TransactionCreateInputParams input = new TransactionCreateInputParams();
            input.ClientId = 5;
            input.CurrencyTo = "EUR";
            input.CurrencyFrom = "TRA";
            input.Value = 8;

            TransactionCreateResult result;

            using (var context = new ConvertTransactionContext(DbContext()))
            {
                var repository = new CurrencyConverterRepository(context, MyConfiguration(), new Mock<ILogger<CurrencyConverterRepository>>().Object);

                result = await repository.insertConversionTransactionAsync(input);
            }

            result.ErrorDetails.Should().NotBeNull();
            result.ErrorDetails.Should().BeOfType<TransactionError>().Subject.Code.Should().Be(1005);
        }


        [Fact]
        public async Task insertConversionTransactionAsync_WithNoValidCurrency_ReturnsInvalidCurrencys()
        {
            TransactionCreateInputParams input = new TransactionCreateInputParams();
            input.ClientId = 5;
            input.CurrencyTo = "ASD";
            input.CurrencyFrom = "TRA";
            input.Value = 8;

            TransactionCreateResult result;

            using (var context = new ConvertTransactionContext(DbContext()))
            {
                var repository = new CurrencyConverterRepository(context, MyConfiguration(), new Mock<ILogger<CurrencyConverterRepository>>().Object);

                result = await repository.insertConversionTransactionAsync(input);
            }

            result.ErrorDetails.Should().NotBeNull();
            result.ErrorDetails.Should().BeOfType<TransactionError>().Subject.Code.Should().Be(1005);
        }

        [Fact]
        public async Task getAllTransactionsByUserIdAsync_WithNoUserId_ReturnsInvalidUser()
        {
            TransactionUserResult result;
            using (var context = new ConvertTransactionContext(DbContext()))
            {
                var repository = new CurrencyConverterRepository(context, MyConfiguration(), new Mock<ILogger<CurrencyConverterRepository>>().Object);

                result = await repository.getAllTransactionsByUserIdAsync(1);
            }

            result.ErrorDetails.Should().NotBeNull();
            result.ErrorDetails.Should().BeOfType<TransactionError>().Subject.Code.Should().Be(1001);
        }

        [Fact]
        public async Task getAllTransactionsByUserIdAsync_WithContainsUserId_ReturnsValidUser()
        {
            TransactionUserResult result;
            ConvertTransaction transaction = new ConvertTransaction();
            transaction.UserId = 1;
            transaction.CurrencyTo = "EUR";
            transaction.CurrencyFrom = "USD";
            transaction.Value = 5;
            transaction.ConversionRate = 1.21387;
            transaction.QuoteRate = 1.21387;
            transaction.DateHourUTC = DateTime.UtcNow;
            using (var context = new ConvertTransactionContext(DbContext()))
            {
                context.Add(transaction);
                context.SaveChanges();

                var repository = new CurrencyConverterRepository(context, MyConfiguration(), new Mock<ILogger<CurrencyConverterRepository>>().Object);
                result = await repository.getAllTransactionsByUserIdAsync(1);
            }

            result.Should().NotBeNull();
            result.Details.Should().NotBeNull();
        }
    }
}