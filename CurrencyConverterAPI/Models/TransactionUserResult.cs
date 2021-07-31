using System;
using System.Collections.Generic;

namespace CurrencyConverterAPI.Models
{
    public class TransactionUserResult
    {
        public int UserId { get; set; }
        public List<UserDetails> Details { get; set; }
        public TransactionError ErrorDetails { get; set; }
    }

    public class UserDetails
    {
        public string CurrencyTo { get; set; }
        public double Value { get; set; }
        public string CurrencyFrom { get; set; }
        public double ConversionRate { get; set; }
        public double ConvertedValue { get; set; }
        public DateTime DateHourUtc { get; set; }
    }
}
