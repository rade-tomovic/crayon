using CurrencyConverter.Application.Models;

namespace CurrencyConverter.Application.Interfaces;

public interface ICurrencyConversionTimeRangeResult
{
    CurrencyDateContainer MaxRateInformation { get;  }
    CurrencyDateContainer MinRateInformation { get; }
    decimal AverageRate { get;  }
}