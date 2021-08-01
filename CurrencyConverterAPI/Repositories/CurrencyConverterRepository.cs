using CurrencyConverterAPI.Data;
using CurrencyConverterAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverterAPI.Repositories
{
    public class CurrencyConverterRepository : ICurrencyConverterRepository
    {
        private readonly ConvertTransactionContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CurrencyConverterRepository> _logger;

        public CurrencyConverterRepository(ConvertTransactionContext context, IConfiguration configuration, ILogger<CurrencyConverterRepository> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<TransactionCreateResult> insertConversionTransactionAsync(TransactionCreateInputParams input)
        {
            TransactionCreateResult result = new TransactionCreateResult();
            string baseUrl = _configuration.GetSection("AppSettings").GetSection("BASE_URL").Value;
            string accessKey = _configuration.GetSection("AppSettings").GetSection("ACCESS_KEY").Value;
            string symbols = input.CurrencyFrom + "," + input.CurrencyTo;

            ExchangeRate recoveryRates = getRates(baseUrl, accessKey, symbols);

            if (recoveryRates.rates != null && recoveryRates.rates.Count < 2)
            {
                string message = "Moeda inválida para conversão: ";
                string messageRates = recoveryRates.rates.First().Key != input.CurrencyFrom ? input.CurrencyFrom : input.CurrencyTo;
                result.ErrorDetails = getError(1005, message + messageRates);
                return result;
            }

            else if (!recoveryRates.success)
            {
                if (recoveryRates.error.code == 202)
                {
                    string message = "Invalid Currencies for Conversion: " + input.CurrencyFrom + ", " + input.CurrencyTo;
                    result.ErrorDetails = getError(1005, message);
                    return result;
                }
                else
                {
                    string message = "Error ExternalAPI exchangeratesapi: " + "Code API: " + recoveryRates.error.code;
                    result.ErrorDetails = getError(1002, message);
                    return result;
                }
            }

            _logger.LogInformation($"Url: {baseUrl} " + $"Symbols: {symbols} " + "Fees: ");
            foreach (KeyValuePair<string, double> rate in recoveryRates.rates)
            {
                _logger.LogInformation($"Currency: {rate.Key} " + $"Factor: {rate.Value} ");
            }

            CalcConversion calcConversion = new CalcConversion();
            calcConversion.value = input.Value;
            calcConversion.valueCurrencyFrom = recoveryRates.rates[input.CurrencyFrom];
            calcConversion.valueCurrencyTo = recoveryRates.rates[input.CurrencyTo];

            result.ValueTo = calcConversion.calculateConversion(calcConversion);

            result.ConversionRate = calcConversion.valueConversionRate;

            result.TransactionId = Guid.NewGuid();
            result.UserId = input.ClientId;
            result.CurrencyFrom = input.CurrencyFrom;
            result.ValueFrom = input.Value;
            result.CurrencyTo = input.CurrencyTo;

            result.DateHourUTC = DateTime.UtcNow;

            await insertConvertTransaction(result, calcConversion.valueQuoteRate);

            return result;
        }

        public async Task<TransactionUserResult> getAllTransactionsByUserIdAsync(int userId)
        {
            TransactionUserResult result = new TransactionUserResult();
            _logger.LogInformation($"Searching user transactions with ID: {userId}");
            List<ConvertTransaction> listUserConvertTransaction = await _context.ConvertTransactions.Where(x => x.UserId == userId).ToListAsync();

            if (listUserConvertTransaction == null || listUserConvertTransaction.Count < 1)
            {
                string message = $"There is no transaction for the user ID: {userId}";
                result.ErrorDetails = getError(1001, message);
                return result;
            }

            result.UserId = userId;
            result.Details = new List<UserDetails>();
            UserDetails details;

            foreach (var item in listUserConvertTransaction)
            {
                details = new()
                {
                    CurrencyFrom = item.CurrencyFrom,
                    Value = item.Value,
                    CurrencyTo = item.CurrencyTo,
                    ConversionRate = item.ConversionRate,
                    ConvertedValue = item.Value * item.QuoteRate,
                    DateHourUtc = item.DateHourUTC
                };

                result.Details.Add(details);
            }

            return result;
        }

        private async Task insertConvertTransaction(TransactionCreateResult transaction, double quotaRate)
        {
            ConvertTransaction convertTransaction = new ConvertTransaction();
            convertTransaction.UserId = transaction.UserId;
            convertTransaction.CurrencyFrom = transaction.CurrencyFrom;
            convertTransaction.Value = transaction.ValueFrom;
            convertTransaction.CurrencyTo = transaction.CurrencyTo;
            convertTransaction.ConversionRate = transaction.ConversionRate;
            convertTransaction.QuoteRate = quotaRate;
            convertTransaction.DateHourUTC = transaction.DateHourUTC;
            _context.Add(convertTransaction);
            await _context.SaveChangesAsync();
        }

        private ExchangeRate getRates(string baseUrl, string accessKey, string symbols)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("access_key", accessKey);
            request.AddParameter("symbols", symbols);

            _logger.LogInformation($"Calling external api: {baseUrl} ");

            IRestResponse response = client.Execute(request);

            _logger.LogInformation($"API response : {JsonConvert.DeserializeObject<ExchangeRate>(response.Content)}");

            return JsonConvert.DeserializeObject<ExchangeRate>(response.Content);
        }

        private TransactionError getError(int code, string message)
        {
            TransactionError error = new()
            {
                Error = true,
                Code = code,
                Message = message
            };
            _logger.LogError(message);

            return error;
        }
    }
}
