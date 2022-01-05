namespace CurrencyConverter.Application.Interfaces;

public interface ICurrencyConverter
{
    CurrencyConversionTimeRangeResult GetConversionForTimeSeries(CurrencyConversionTimeRangeInputModel model);
}