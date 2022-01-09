using CurrencyConverter.Application.Interfaces;
using CurrencyConverter.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CurrencyConverter.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyConversionController : ControllerBase
{
    private readonly ICurrencyConverter _currencyConverter;

    public CurrencyConversionController(ICurrencyConverter currencyConverter)
    {
        _currencyConverter = currencyConverter;
    }

    /// <summary>
    /// Fetches minimum, maximum and average exchange rates for provided currencies
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST date-range-indicators
    ///     {
    ///        "dateSet": [
    ///             "2018-02-01",
    ///             "2018-02-15",
    ///             "2018-03-01"
    ///        ],
    ///        "baseCurrency": "SEK",
    ///        "targetCurrency": "NOK"
    ///     }
    /// </remarks>
    /// <returns>Minimum, maximum and average exchange rates for provided currencies and set of dates</returns>
    /// <response code="200">Returns the calculation result</response>
    /// <response code="400">Returns invalid model if input model has validation errors</response> 
    /// <response code="404">Returns null model if  response has errors</response> 
    [HttpPost("date-range-indicators")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyConversionTimeRangeResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CurrencyConversionTimeRangeNullResult))]
    [SwaggerOperation(OperationId = "getCurrencyIndicatorsForDateRange",
        Summary = "Get currency indicators for date range")]
    public async Task<IActionResult> GetCurrencyIndicatorsForDateRange(CurrencyConversionTimeRangeInputModel model)
    {
        ICurrencyConversionTimeRangeResult result = await _currencyConverter.GetCurrencyIndicatorsForSetOfDates(model);
        return result.AverageRate != 0 ? Ok(result) : NotFound(result);
    }
}