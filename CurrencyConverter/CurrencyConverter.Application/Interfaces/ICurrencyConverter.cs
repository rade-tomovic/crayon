using CurrencyConverter.Application.Models;

namespace CurrencyConverter.Application.Interfaces;

public interface ICurrencyConverter
{
    Task<ICurrencyConversionTimeRangeResult> GetCurrencyIndicatorsForSetOfDates(CurrencyConversionTimeRangeInputModel model);
}