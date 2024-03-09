using Serilog.Context;

namespace Bookify.Api.Middleware;

public class RequestContextLoggingMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeader = "X-Correlation-Id";
    
    public Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("CorrelationId", 
                       context.Request.Headers[CorrelationIdHeader].FirstOrDefault() 
                       ?? context.TraceIdentifier))
        {
            return next(context);
        }
    }
}