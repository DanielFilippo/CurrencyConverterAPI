namespace CurrencyConverterAPI.Models
{
    public class TransactionError
    {
        public bool? Error { get; set; }
        public int? Code { get; set; }
        public string Message { get; set; }
    }
}
