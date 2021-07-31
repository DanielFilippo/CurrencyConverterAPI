using System;

namespace CurrencyConverterAPI.Models
{
    public class TransactionCreateResult
    {
        public Guid TransactionId { get; set; }
        public int UserId { get; set; }
        public string CurrencyTo { get; set; }
        public double ValueTo { get; set; }
        public string CurrencyFrom { get; set; }
        public double ValueFrom { get; set; }
        public double ConversionRate { get; set; }
        public DateTime DateHourUTC { get; set; }
        public TransactionError ErrorDetails { get; set; }
    }
}
