using System.Net;
using KafkaDelivery.App.Wrappers;

namespace Web.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            var apiResult = AppResult<string>.Failure(ex.Message);
            await context.Response.WriteAsJsonAsync(apiResult);
        }
    }
}