using System.ComponentModel.DataAnnotations;

namespace CurrencyConverterAPI.Models
{
    public class TransactionCreateInputParams
    {
        [Required]
        public int ClientId { get; set; }
        [Required]
        public string CurrencyTo { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public string CurrencyFrom { get; set; }
    }
}
