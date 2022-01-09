using CurrencyConverter.Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RestSharp;

namespace CurrencyConverter.Application;

public class CurrencyConversionRequestGenerator
{
    private readonly IOptions<CurrencyServiceOptions> _configuration;
    private readonly ILogger<CurrencyConversionService> _logger;

    public CurrencyConversionRequestGenerator(ILogger<CurrencyConversionService> logger, IOptions<CurrencyServiceOptions> configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public List<Task<PolicyResult<IRestResponse<ExchangeRateApiResponse>>>> GenerateRequestTasks(CurrencyConversionTimeRangeInputModel model)
    {
        CurrencyServiceOptions? currencyExchangeOptions = _configuration.Value;

        return model.DateSet.Select(date => Policy
            .Handle<Exception>()
            .OrResult<IRestResponse<ExchangeRateApiResponse>>(r => !r.IsSuccessful)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (response, _, retryAttempt, _) =>
                {
                    if (response.Exception != null)
                        _logger.LogError(response.Exception, "An exception occurred on retry {RetryAttempt}", retryAttempt);
                    else
                        _logger.LogError("A non success code {StatusCode} was received on retry {RetryAttempt}",
                            (int)response.Result.StatusCode, retryAttempt);
                })
            .ExecuteAndCaptureAsync(() => GenerateRequestToExchangeRateApi(model, date, currencyExchangeOptions))).ToList();
    }

    private Task<IRestResponse<ExchangeRateApiResponse>> GenerateRequestToExchangeRateApi(CurrencyConversionTimeRangeInputModel model,
        DateTimeOffset date, CurrencyServiceOptions? currencyExchangeOptions)
    {
        string dateForConversion = $"{date:yyyy-MM-dd}";
        IRestClient client = new RestClient($"{currencyExchangeOptions!.BaseUrl}{dateForConversion}");
        var request = new RestRequest
        {
            Method = Method.GET
        };

        request.AddParameter("base", model.BaseCurrency, ParameterType.QueryString);
        request.AddParameter("symbols", model.TargetCurrency, ParameterType.QueryString);
        request.AddParameter("places", currencyExchangeOptions.DecimalPlaces, ParameterType.QueryString);
        _logger.LogInformation("Request URI: {0}", client.BuildUri(request));

        return client.ExecuteAsync<ExchangeRateApiResponse>(request);
    }
}