using CurrencyConverter.Application.Interfaces;

namespace CurrencyConverter.Application;

public class CurrencyConversionService : ICurrencyConverter
{
    public async Task<CurrencyConversionTimeRangeResult> GetCurrencyIndicatorsForSetOfDates(CurrencyConversionTimeRangeInputModel model)
    {
        throw new NotImplementedException();
    }
}