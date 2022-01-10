using CurrencyConverter.Application.Models;
using FluentValidation;

namespace CurrencyConverter.Application.Validation;

public class CurrencyConversionTimeRangeInputModelValidator : AbstractValidator<CurrencyConversionTimeRangeInputModel>
{
    public CurrencyConversionTimeRangeInputModelValidator()
    {
        RuleFor(inputModel => inputModel.DateSet)
            .NotNull()
            .NotEmpty();

        RuleFor(inputModel => inputModel.BaseCurrency)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(3);

        RuleFor(inputModel => inputModel.TargetCurrency)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(3);
    }
}