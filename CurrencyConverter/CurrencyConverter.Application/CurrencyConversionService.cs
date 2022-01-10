using CurrencyConverter.Application.Interfaces;
using CurrencyConverter.Application.Models;
using CurrencyConverter.Application.Validation;
using FluentValidation;
using FluentValidation.Results;
using Polly;
using RestSharp;

namespace CurrencyConverter.Application;

public class CurrencyConversionService : ICurrencyConverter
{
    private readonly CurrencyConversionRequestGenerator _requestGenerator;

    public CurrencyConversionService(CurrencyConversionRequestGenerator requestGenerator)
    {
        _requestGenerator = requestGenerator;
    }

    public async Task<ICurrencyConversionTimeRangeResult> GetCurrencyIndicatorsForSetOfDates(CurrencyConversionTimeRangeInputModel model)
    {
        ValidateModel(model);
        PrepareInputData(model);
        List<Task<PolicyResult<IRestResponse<ExchangeRateApiResponse>>>> requestTasks = _requestGenerator.GenerateRequestTasks(model);
        List<ExchangeRateApiResponse> results = new();

        await Task.WhenAll(requestTasks).ContinueWith(taskResponses =>
        {
            results = taskResponses.Result.Select(x => x.Result.Data).ToList();
            ValidateResponse(results, model);
        });

        return results is { Count: > 0 } ? CalculateIndicators(results, model) : new CurrencyConversionTimeRangeNullResult();
    }

    private static void PrepareInputData(CurrencyConversionTimeRangeInputModel model)
    {
        model.DateSet = model.DateSet.ToHashSet(new DateComparer()).ToList();
        model.BaseCurrency = model.BaseCurrency.ToUpper();
        model.TargetCurrency = model.TargetCurrency.ToUpper();
    }

    private static void ValidateModel(CurrencyConversionTimeRangeInputModel model)
    {
        var modelValidator = new CurrencyConversionTimeRangeInputModelValidator();
        modelValidator.ValidateAndThrow(model);
    }

    private static void ValidateResponse(List<ExchangeRateApiResponse> responses, CurrencyConversionTimeRangeInputModel inputModel)
    {
        List<ValidationResult> validationResults = new();

        foreach (ExchangeRateApiResponse response in responses)
        {
            var validator = new ExchangeRateApiResponseValidator(inputModel);
            ValidationResult? validationResult = validator.Validate(response);
            if (validationResult != null)
                validationResults.Add(validationResult);
        }

        if (validationResults.Any(x => !x.IsValid))
            throw new ValidationException("API response is not valid", validationResults.SelectMany(x => x.Errors));
    }

    private static CurrencyConversionTimeRangeResult CalculateIndicators(List<ExchangeRateApiResponse> apiResponses,
        CurrencyConversionTimeRangeInputModel model)
    {
        Dictionary<DateTime, decimal> ratesPerDate = GetRatesPerDate(apiResponses, model);

        return new CurrencyConversionTimeRangeResult
        {
            AverageRate = ratesPerDate.Values.Average(),
            MaxRateInformation = new CurrencyDateContainer
            {
                HistoricalDate = ratesPerDate.Aggregate((left, right) => left.Value > right.Value ? left : right).Key,
                CurrencyExchangeRate = ratesPerDate.Values.Max()
            },
            MinRateInformation = new CurrencyDateContainer
            {
                HistoricalDate = ratesPerDate.Aggregate((left, right) => left.Value < right.Value ? left : right).Key,
                CurrencyExchangeRate = ratesPerDate.Values.Min()
            }
        };
    }

    private static Dictionary<DateTime, decimal> GetRatesPerDate(List<ExchangeRateApiResponse> apiResponses,
        CurrencyConversionTimeRangeInputModel model)
    {
        return apiResponses.Select(x => new
        {
            x.Date,
            Rate = Convert.ToDecimal(((Dictionary<string, object>)x.Rates)[model.TargetCurrency])
        }).ToDictionary(x => DateTime.Parse(x.Date), x => x.Rate);
    }
}