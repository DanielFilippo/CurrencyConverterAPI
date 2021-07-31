using System;

namespace CurrencyConverterAPI.Models
{
    public class CalcConversion
    {

        public double value { get; set; }
        public double valueCurrencyTo { get; set; }
        public double valueCurrencyFrom { get; set; }
        public double valueConversionRate { get; set; }
        public double valueQuoteRate { get; set; }

        public CalcConversion()
        {

        }

        public CalcConversion(double value, double valueCurrencyTo, double valueCurrencyFrom)
        {
            this.value = value;
            this.valueCurrencyTo = valueCurrencyTo;
            this.valueCurrencyFrom = valueCurrencyFrom;
        }

        public double calculateConversion(CalcConversion calcConversion)
        {
            if (calcConversion.valueCurrencyTo > calcConversion.valueCurrencyFrom)
            {
                calcConversion.valueConversionRate = calcConversion.valueCurrencyTo / calcConversion.valueCurrencyFrom;
                valueQuoteRate = Math.Round((calcConversion.value / (calcConversion.valueCurrencyTo / calcConversion.valueCurrencyFrom)) / value, 4);
                return Math.Round(calcConversion.value / (calcConversion.valueCurrencyTo / calcConversion.valueCurrencyFrom), 4);
            }
            else if (calcConversion.valueCurrencyTo < calcConversion.valueCurrencyFrom)
            {
                calcConversion.valueConversionRate = calcConversion.valueCurrencyFrom / calcConversion.valueCurrencyTo;
                valueQuoteRate = Math.Round(((calcConversion.valueCurrencyFrom / calcConversion.valueCurrencyTo) * calcConversion.value) / value, 4);
                return Math.Round((calcConversion.valueCurrencyFrom / calcConversion.valueCurrencyTo) * calcConversion.value, 4);
            }
            else
            {
                calcConversion.valueConversionRate = calcConversion.valueCurrencyTo / calcConversion.valueCurrencyFrom;
                valueQuoteRate = Math.Round(((calcConversion.valueCurrencyTo / calcConversion.valueCurrencyFrom) * calcConversion.value) / value, 4);
                return Math.Round((calcConversion.valueCurrencyTo / calcConversion.valueCurrencyFrom) * calcConversion.value, 4);
            }
        }
    }
}