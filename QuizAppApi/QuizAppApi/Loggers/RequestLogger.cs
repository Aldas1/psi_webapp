using Serilog;

namespace QuizAppApi.Loggers;

public class RequestLogger
{
    private readonly Serilog.ILogger _logger = new LoggerConfiguration()
        .WriteTo.File("requests.txt", flushToDiskInterval: TimeSpan.Zero)
        .CreateLogger();

    public void LogRequest(HttpRequest request, HttpResponse response, long millisecondsTime)
    {
        _logger.Information("{requestMethod} {requestPath} - {responseStatus} ({time} ms)", request.Method, request.Path, response.StatusCode, millisecondsTime);
    }
}