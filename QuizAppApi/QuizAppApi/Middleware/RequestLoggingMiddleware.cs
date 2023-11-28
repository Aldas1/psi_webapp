using System.Diagnostics;
using QuizAppApi.Loggers;

namespace QuizAppApi.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RequestLogger _requestLogger;

    public RequestLoggingMiddleware(RequestDelegate next, RequestLogger requestLogger)
    {
        _next = next;
        _requestLogger = requestLogger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        await _next(context);
        stopwatch.Stop();
        _requestLogger.LogRequest(context.Request, context.Response, stopwatch.ElapsedMilliseconds);
    }
    
}