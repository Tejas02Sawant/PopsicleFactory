using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace PopsicleFactory.Infrastructure.Middlewares;

public class RequestContextLoggingMiddleware(RequestDelegate _next, ILogger<RequestContextLoggingMiddleware> _logger)
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    public async Task Invoke(HttpContext context)
    {
        string correlationId = GetCorrelationId(context);

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
            await _next.Invoke(context);
            _logger.LogInformation($"Response: {context.Response.StatusCode}");
        }
    }

    private static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(
            CorrelationIdHeaderName, out StringValues correlationId);

        return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
    }
}
