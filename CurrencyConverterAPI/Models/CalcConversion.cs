using System;

namespace CurrencyConverterAPI.Models
{
    public class CalcConversion
    {

        public double value { get; set; }
        public double valueCurrencyFrom { get; set; }
        public double valueCurrencyTo { get; set; }
        public double valueConversionRate { get; set; }
        public double valueQuoteRate { get; set; }

        public CalcConversion()
        {

        }

        public CalcConversion(double value, double valueCurrencyFrom, double valueCurrencyTo)
        {
            this.value = value;
            this.valueCurrencyFrom = valueCurrencyFrom;
            this.valueCurrencyTo = valueCurrencyTo;
        }

        public double calculateConversion(CalcConversion calcConversion)
        {
            if (calcConversion.valueCurrencyFrom > calcConversion.valueCurrencyTo)
            {
                calcConversion.valueConversionRate = calcConversion.valueCurrencyFrom / calcConversion.valueCurrencyTo;
                valueQuoteRate = Math.Round((calcConversion.value / (calcConversion.valueCurrencyFrom / calcConversion.valueCurrencyTo)) / value, 4);
                return Math.Round(calcConversion.value / (calcConversion.valueCurrencyFrom / calcConversion.valueCurrencyTo), 4);
            }
            else if (calcConversion.valueCurrencyFrom < calcConversion.valueCurrencyTo)
            {
                calcConversion.valueConversionRate = calcConversion.valueCurrencyTo / calcConversion.valueCurrencyFrom;
                valueQuoteRate = Math.Round(((calcConversion.valueCurrencyTo / calcConversion.valueCurrencyFrom) * calcConversion.value) / value, 4);
                return Math.Round((calcConversion.valueCurrencyTo / calcConversion.valueCurrencyFrom) * calcConversion.value, 4);
            }
            else
            {
                calcConversion.valueConversionRate = calcConversion.valueCurrencyFrom / calcConversion.valueCurrencyTo;
                valueQuoteRate = Math.Round(((calcConversion.valueCurrencyFrom / calcConversion.valueCurrencyTo) * calcConversion.value) / value, 4);
                return Math.Round((calcConversion.valueCurrencyFrom / calcConversion.valueCurrencyTo) * calcConversion.value, 4);
            }
        }
    }
}