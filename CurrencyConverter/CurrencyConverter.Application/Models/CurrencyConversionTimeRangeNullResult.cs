using CurrencyConverter.Application.Interfaces;

namespace CurrencyConverter.Application.Models;

public class CurrencyConversionTimeRangeNullResult : ICurrencyConversionTimeRangeResult
{
    public CurrencyDateContainer MaxRateInformation =>
        new()
        {
            CurrencyExchangeRate = 0,
            HistoricalDate = DateTime.MinValue
        };

    public CurrencyDateContainer MinRateInformation => new()
    {
        CurrencyExchangeRate = 0,
        HistoricalDate = DateTime.MinValue
    };

    public decimal AverageRate => 0;
}