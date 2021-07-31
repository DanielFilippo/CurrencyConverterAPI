using System.Collections.Generic;

namespace CurrencyConverterAPI.Models
{
    public class ExchangeRate
    {
        public bool success { get; set; }
        public Dictionary<string, double> rates { get; set; }
        public Error error { get; set; }
    }

    public class Error
    {
        public int? code { get; set; }
        public string type { get; set; }
        public string info { get; set; }
    }
}
