using Newtonsoft.Json;

namespace CurrencyConverter.Application.Models;

public class Motd
{
    [JsonProperty("msg")]
    public string Msg { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
}

public class ExchangeRateApiResponse
{
    [JsonProperty("motd")]
    public Motd Motd { get; set; }

    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("historical")]
    public bool Historical { get; set; }

    [JsonProperty("base")]
    public string Base { get; set; }

    [JsonProperty("date")]
    public string Date { get; set; }

    [JsonProperty("rates")]

    public object Rates { get; set; }
}