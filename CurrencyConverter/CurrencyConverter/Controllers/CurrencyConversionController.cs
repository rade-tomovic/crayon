using CurrencyConverter.Application;
using CurrencyConverter.Application.Interfaces;
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

    [HttpGet("date-range-indicators")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CurrencyConversionTimeRangeResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(OperationId = "getCurrencyIndicatorsForDateRange",
        Summary = "Get currency indicators for date range")]
    public async Task<IActionResult> GetCurrencyIndicatorsForDateRange(CurrencyConversionTimeRangeInputModel model)
    {
        Task<CurrencyConversionTimeRangeResult> result = _currencyConverter.GetCurrencyIndicatorsForSetOfDates(model);
        return Ok(result);
    }
}