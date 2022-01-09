namespace CurrencyConverter.Application;

public class CurrencyServiceOptions
{
    public const string SectionName = "CurrencyServiceOptions";

    public short DecimalPlaces { get; set; }
    public string BaseUrl { get; set; }
}