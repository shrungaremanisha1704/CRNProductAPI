using System.Net;
using System.Text.Json;

namespace CRNProductAPI.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            if (context.Response.HasStarted) throw;
            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                statusCode = 500,
                message = "An unexpected error occurred. Please try again later.",
                traceId = context.TraceIdentifier
            }));
        }
    }
}
