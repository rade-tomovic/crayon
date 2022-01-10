using CurrencyConverter.Application.Interfaces;

namespace CurrencyConverter.Application.Models;

public class CurrencyConversionTimeRangeResult : ICurrencyConversionTimeRangeResult
{
    public CurrencyDateContainer MaxRateInformation { get; init; }
    public CurrencyDateContainer MinRateInformation { get; init; }
    public decimal AverageRate { get; init; }
}