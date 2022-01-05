namespace CurrencyConverter.Application.Interfaces;

public interface ICurrencyConverter
{
    Task<CurrencyConversionTimeRangeResult> GetCurrencyIndicatorsForSetOfDates(CurrencyConversionTimeRangeInputModel model);
}