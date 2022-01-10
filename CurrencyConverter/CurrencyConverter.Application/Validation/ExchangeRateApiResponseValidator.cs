using CurrencyConverter.Application.Models;
using FluentValidation;

namespace CurrencyConverter.Application.Validation;

public class ExchangeRateApiResponseValidator : AbstractValidator<ExchangeRateApiResponse>
{
    public ExchangeRateApiResponseValidator(CurrencyConversionTimeRangeInputModel inputModel)
    {
        RuleFor(x => x.Success).Equal(true);
        RuleFor(x => x.Historical).Equal(true);
        RuleFor(x => x.Base).Equal(inputModel.BaseCurrency);
        RuleFor(x => x.Rates).NotNull();
    }
}