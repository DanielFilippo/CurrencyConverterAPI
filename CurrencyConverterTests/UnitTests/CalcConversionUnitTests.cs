using CurrencyConverterAPI.Models;
using Xunit;
using FluentAssertions;

namespace CurrencyConverterUnitTests.UnitTests
{
    public class CalcConversionUnitTests
    {
        [Theory]
        [InlineData(5, 1, 6.052449, 30.26)] //EUR_BRL
        [InlineData(5, 6.052449, 1, 0.82)] //BRL_EUR
        [InlineData(5, 6.052449, 1.184827, 0.97)] //BRL_USD
        [InlineData(5, 1.184827, 6.052449, 25.54)] //USD_BRL
        [InlineData(5, 130.173384, 1.184827, 0.04)] //JPY_USD
        [InlineData(5, 1.184827, 130.173384, 549.33)] //USD_JPY
        [InlineData(5, 5, 5, 5)] //SomeFactors
        public void CalcConversion_CalculateConversion_ReturnSucess(double value, double valueCurrencyFrom, double valueCurrencyTo, double expectedResult)
        {
            CalcConversion calcConversion = new CalcConversion();
            double result = calcConversion.calculateConversion(new CalcConversion(value, valueCurrencyFrom, valueCurrencyTo));

            result.Should().BePositive();
            result.Should().BeApproximately(expectedResult, 0.10);
        }
    }
}
