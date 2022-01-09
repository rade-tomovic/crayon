using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurrencyConverter.Application;
using CurrencyConverter.Application.Interfaces;
using CurrencyConverter.Application.Models;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CurrencyConverter.Tests;

public class CurrencyConversionServiceTests
{
    private readonly IOptions<CurrencyServiceOptions> _configuration;
    private readonly ILogger<CurrencyConversionService> _logger;

    public CurrencyConversionServiceTests()
    {
        _configuration = Options.Create(new CurrencyServiceOptions
        {
            BaseUrl = "https://api.exchangerate.host/",
            DecimalPlaces = 6
        });
        _logger = Mock.Of<ILogger<CurrencyConversionService>>();
    }

    [Fact]
    public async Task CurrencyConversionService_ForProvidedActualData_ShouldCalculateIndicatorsCorrectly()
    {
        var currencyConversionRequestGenerator = new CurrencyConversionRequestGenerator(_logger, _configuration);
        var currencyConversionService = new CurrencyConversionService(currencyConversionRequestGenerator);
        var modelForTest = new CurrencyConversionTimeRangeInputModel
        {
            BaseCurrency = "SEK",
            TargetCurrency = "NOK",
            DateSet = new List<DateTime>
            {
                new(2018, 2, 1),
                new(2018, 2, 15),
                new(2018, 3, 1)
            }
        };

        ICurrencyConversionTimeRangeResult result = await currencyConversionService.GetCurrencyIndicatorsForSetOfDates(modelForTest);

        result.Should().BeAssignableTo<CurrencyConversionTimeRangeResult>();
    }

    [Fact]
    public async Task CurrencyConversionService_ForInputWithoutDates_ShouldThrow()
    {
        var currencyConversionRequestGenerator = new CurrencyConversionRequestGenerator(_logger, _configuration);
        var currencyConversionService = new CurrencyConversionService(currencyConversionRequestGenerator);
        

        Func<Task<ICurrencyConversionTimeRangeResult>> f = async () =>
        {
            var modelForTest = new CurrencyConversionTimeRangeInputModel
            {
                BaseCurrency = "SEK",
                TargetCurrency = "NOK",
                DateSet = null
            };

            return await currencyConversionService.GetCurrencyIndicatorsForSetOfDates(modelForTest);
        };


        await f.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CurrencyConversionService_ForInputWithoutCurrencies_ShouldThrow()
    {
        var currencyConversionRequestGenerator = new CurrencyConversionRequestGenerator(_logger, _configuration);
        var currencyConversionService = new CurrencyConversionService(currencyConversionRequestGenerator);


        Func<Task<ICurrencyConversionTimeRangeResult>> f = async () =>
        {
            var modelForTest = new CurrencyConversionTimeRangeInputModel
            {
                BaseCurrency = null,
                TargetCurrency = string.Empty,
                DateSet = new List<DateTime>
                {
                    new(2018, 2, 1),
                    new(2018, 2, 15),
                    new(2018, 3, 1)
                }
            };

            return await currencyConversionService.GetCurrencyIndicatorsForSetOfDates(modelForTest);
        };


        await f.Should().ThrowAsync<ValidationException>();
    }
}