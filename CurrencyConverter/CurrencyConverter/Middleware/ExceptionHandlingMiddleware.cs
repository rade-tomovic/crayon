using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {

        ProblemDetails problemDetails = new GlobalExceptionHandler(context, exception, _logger)
            .PrepareErrorResponse()
            .LogException()
            .CreateProblemDetails();

        return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}