using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.API.Middleware;

public class GlobalExceptionHandler
{
    private readonly HttpContext _context;
    private readonly Exception _exception;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(HttpContext context, Exception exception, ILogger<GlobalExceptionHandler> logger)
    {
        _context = context;
        _exception = exception;
        _logger = logger;
    }

    internal GlobalExceptionHandler PrepareErrorResponse()
    {
        _context.Response.Clear();
        _context.Response.ContentType = "application/json";
        return this;
    }

    internal GlobalExceptionHandler LogException()
    {
        _logger.LogError(_exception, "Global error handler: {0}", _exception.Message);

        return this;
    }

    internal ProblemDetails CreateProblemDetails()
    {
        switch (_exception)
        {
            case ArgumentNullException _:
            case ArgumentException _:
                {
                    const int statusCode = (int)HttpStatusCode.BadRequest;
                    _context.Response.StatusCode = statusCode;

                    return new ProblemDetails
                    {
                        Title = "Invalid argument",
                        Status = statusCode,
                        Detail = _exception.Message,
                        Type = _exception.GetType().ToString()
                    };
                }
            case HttpRequestException _:
                {
                    const int statusCode = (int)HttpStatusCode.BadRequest;
                    _context.Response.StatusCode = statusCode;

                    return new ProblemDetails
                    {
                        Title = "Invalid HTTP request",
                        Status = statusCode,
                        Detail = _exception.Message,
                        Type = _exception.GetType().ToString()
                    };
                }
            case UnauthorizedAccessException _:
                {
                    const int statusCode = (int)HttpStatusCode.Unauthorized;
                    _context.Response.StatusCode = statusCode;

                    return new ProblemDetails
                    {
                        Title = "Invalid user identity",
                        Status = statusCode,
                        Detail = _exception.Message,
                        Type = _exception.GetType().ToString()
                    };
                }

            default:
                {
                    const int statusCode = (int)HttpStatusCode.InternalServerError;
                    _context.Response.StatusCode = statusCode;

                    return new ProblemDetails
                    {
                        Title = "Something went wrong",
                        Status = statusCode,
                        Detail = _exception.InnerException?.Message ?? _exception.Message,
                        Type = _exception.GetType().ToString()
                    };
                }
        }
    }
}