namespace CurrencyConverter.Application.Models;

public class CurrencyConversionTimeRangeInputModel
{
    public List<DateTime> DateSet { get; set; }
    public string BaseCurrency { get; set; }
    public string TargetCurrency { get; set; }
}