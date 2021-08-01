using CurrencyConverterAPI.Models;
using CurrencyConverterAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace CurrencyConverterAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly ICurrencyConverterRepository _repository;
        private readonly ILogger<CurrencyConverterController> _logger;

        public CurrencyConverterController(ICurrencyConverterRepository repository, ILogger<CurrencyConverterController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        /// <summary>
        /// Create a new Transaction, for the currencys you need to put in ISO 4217 format
        /// </summary>
        /// <param name="input">Input parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionCreateResult>> CreateTransaction([FromBody] TransactionCreateInputParams input)
        {
            _logger.LogInformation("Testing CD");
            _logger.LogInformation(
                "New request received on CreateTransaction |" +
               $"ClienteId : {input.ClientId}" +
               $"MoedaDe: {input.CurrencyFrom}|" +
               $"MoedaPara: {input.CurrencyTo}|" +
               $"Valor : {input.Value}|");

            if (input.ClientId <= 0)
                return GenerateResultParamValuesInvalid("User Id");

            if (input.Value <= 0)
                return GenerateResultParamValuesInvalid("value");

            if (string.IsNullOrEmpty(input.CurrencyFrom))
                return GenerateResultParamStringInvalid("origin");

            if (string.IsNullOrEmpty(input.CurrencyFrom))
                return GenerateResultParamStringInvalid("destination");

            TransactionCreateResult result = await _repository.insertConversionTransactionAsync(input);

            if (result is null)
            {
                return NotFound();
            }

            if (result.ErrorDetails != null)
                return GenerateCreateResultError(result.ErrorDetails.Message);

            _logger.LogInformation($"Converted value {result.ValueTo}");

            return CreatedAtAction(nameof(CreateTransaction), result);
        }

        /// <summary>
        /// Get the client response by client id
        /// </summary>
        /// <param name="userId">Client Id</param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionUserResult>> GetAllTransactionsByUserId([FromRoute] int userId)
        {
            _logger.LogInformation("New request received on GetAllTransactionsByUserId " + $"ClienteId : {userId}");
            if (userId <= 0)
                return GenerateResultParamValuesInvalid("User Id");

            TransactionUserResult result = await _repository.getAllTransactionsByUserIdAsync(userId);

            if (result is null)
            {
                return NotFound();
            }

            if (result.ErrorDetails != null)
                return GenerateGetUserIdResultError(result.ErrorDetails.Message);

            return Ok(result);
        }

        private BadRequestObjectResult GenerateGetUserIdResultError(string message)
        {
            _logger.LogError(message);

            LogStatusHttpRequest(HttpStatusCode.BadRequest);

            return new BadRequestObjectResult(
                new TransactionError() { Error = true, Code = 1001, Message = message });
        }

        private BadRequestObjectResult GenerateCreateResultError(string message)
        {
            _logger.LogError(message);

            LogStatusHttpRequest(HttpStatusCode.BadRequest);

            return new BadRequestObjectResult(
                new TransactionError() { Error = true, Code = 1002, Message = message });
        }

        private BadRequestObjectResult GenerateResultParamValuesInvalid(string nameField)
        {
            var error = $"Field {nameField} must be greater than 0!";
            _logger.LogError(error);

            LogStatusHttpRequest(HttpStatusCode.BadRequest);

            return new BadRequestObjectResult(
                new TransactionError() { Error = true, Code = 1003, Message = error });
        }

        private BadRequestObjectResult GenerateResultParamStringInvalid(string nameField)
        {
            var error = $"Need to inform a currency of {nameField}!";
            _logger.LogError(error);

            LogStatusHttpRequest(HttpStatusCode.BadRequest);

            return new BadRequestObjectResult(
                new TransactionError() { Error = true, Code = 1004, Message = error });
        }

        private void LogStatusHttpRequest(HttpStatusCode status)
        {
            var codStatus = (int)status;
            var mensagem = codStatus + " " + status;

            if (codStatus >= 400)
                _logger.LogError(mensagem);
            else
                _logger.LogInformation(mensagem);

        }
    }
}
