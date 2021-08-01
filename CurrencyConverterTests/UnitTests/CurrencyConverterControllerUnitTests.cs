using CurrencyConverterAPI.Controllers;
using CurrencyConverterAPI.Models;
using CurrencyConverterAPI.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.CurrencyConverterUnitTests
{
    public class CurrencyConverterControllerUnitTests
    {
        private readonly Mock<ICurrencyConverterRepository> _repositoryMock = new Mock<ICurrencyConverterRepository>();
        private readonly CurrencyConverterController _controller;

        public CurrencyConverterControllerUnitTests()
        {
            _controller = new CurrencyConverterController(_repositoryMock.Object, new Mock<ILogger<CurrencyConverterController>>().Object);
        }
        [Fact]
        public async Task GetAllTransactionsByUserId_WithUnexistingUser_ReturnsNotFound()
        {
            var result = await _controller.GetAllTransactionsByUserId(1);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAllTransactionsByUserId_WithExistingUser_ReturnsExpectedUser()
        {
            var expectedUser = CreateRandomUser();
            _repositoryMock.Setup(repo => repo.getAllTransactionsByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedUser);

            var result = await _controller.GetAllTransactionsByUserId(1);

            result.Result.Should().BeOfType<OkObjectResult>();
            result.Result.Should().BeOfType<OkObjectResult>().Subject.Value.Should().BeEquivalentTo(expectedUser, options => options.ComparingByMembers<TransactionUserResult>());
        }

        [Fact]
        public async Task InsertConversionTransaction_WithInsertUser_ReturnsInvalidUserId()
        {
            TransactionCreateInputParams input = new TransactionCreateInputParams();
            input.CurrencyFrom = "EUR";
            input.Value = 5;
            input.CurrencyTo = "USD";
            var result = await _controller.CreateTransaction(input);

            var okResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            okResult.StatusCode.Should().Be(400);
            okResult.Value.Should().BeOfType<TransactionError>();
            okResult.Value.Should().BeOfType<TransactionError>().Subject.Code.Should().Be(1003);
            okResult.Value.Should().BeOfType<TransactionError>().Subject.Message.Should().NotBeNullOrEmpty();
        }


        [Fact]
        public async Task InsertConversionTransaction_WithInsertUser_ReturnsInvalidValue()
        {
            TransactionCreateInputParams input = new TransactionCreateInputParams();
            input.ClientId = 2;
            input.CurrencyFrom = "EUR";
            input.Value = -1;
            input.CurrencyTo = "USD";
            var result = await _controller.CreateTransaction(input);

            var okResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            okResult.StatusCode.Should().Be(400);
            okResult.Value.Should().BeOfType<TransactionError>();
            okResult.Value.Should().BeOfType<TransactionError>().Subject.Code.Should().Be(1003);
            okResult.Value.Should().BeOfType<TransactionError>().Subject.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task InsertConversionTransaction_WithInsertUser_ReturnsNoCurrency()
        {
            TransactionCreateInputParams input = new TransactionCreateInputParams();
            input.ClientId = 2;
            input.Value = 5;
            input.CurrencyTo = "USD";
            var result = await _controller.CreateTransaction(input);

            var okResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            okResult.StatusCode.Should().Be(400);
            okResult.Value.Should().BeOfType<TransactionError>();
            okResult.Value.Should().BeOfType<TransactionError>().Subject.Code.Should().Be(1004);
            okResult.Value.Should().BeOfType<TransactionError>().Subject.Message.Should().NotBeNullOrEmpty();
        }

        private TransactionUserResult CreateRandomUser()
        {
            TransactionUserResult result = new TransactionUserResult();
            result.UserId = 10000;
            result.Details = new List<UserDetails>();

            UserDetails details = new UserDetails()
            {
                CurrencyFrom = "BRL",
                CurrencyTo = "USD",
                Value = 10,
                ConversionRate = 5.25555,
                ConvertedValue = 5.45,
                DateHourUtc = DateTime.UtcNow
            };

            result.Details.Add(details);

            return result;
        }
    }
}