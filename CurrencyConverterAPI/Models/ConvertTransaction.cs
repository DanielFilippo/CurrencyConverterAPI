using System;

namespace CurrencyConverterAPI.Models
{
    public class ConvertTransaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CurrencyTo { get; set; }
        public double Value { get; set; }
        public string CurrencyFrom { get; set; }
        public double ConversionRate { get; set; }
        public double QuoteRate { get; set; }
        public DateTime DateHourUTC { get; set; }
    }
}
