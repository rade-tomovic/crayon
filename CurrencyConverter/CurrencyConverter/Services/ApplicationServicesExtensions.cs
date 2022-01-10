using CurrencyConverter.Application;
using CurrencyConverter.Application.Interfaces;

namespace CurrencyConverter.API.Services;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrencyConverter, CurrencyConversionService>();
        services.AddScoped<CurrencyConversionRequestGenerator>();
        
        return services;
    }
}