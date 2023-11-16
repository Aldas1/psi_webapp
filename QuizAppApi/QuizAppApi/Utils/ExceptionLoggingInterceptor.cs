using System;
using Castle.DynamicProxy;
using QuizAppApi.Exceptions;
using Serilog;

namespace QuizAppApi.Utils;

public class ExceptionLoggingInterceptor : IInterceptor
{
    private readonly ILogger<ExceptionLoggingInterceptor> _logger;

    public ExceptionLoggingInterceptor(ILogger<ExceptionLoggingInterceptor> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Intercept(IInvocation invocation)
    {
        try
        {
            invocation.Proceed();
        }
        catch (Exception ex)
        {
            LogException(ex);
            throw;
        }
    }
    
    private void LogException(Exception exception)
    {
        _logger.LogError(exception, "Exception in method {MethodName}", exception.Message);

        if (exception is CustomException customException)
        {
            Log.Information("CustomException details: {Details}, ErrorCode: {ErrorCode}", customException.Message, customException.ErrorCode);
        }
    }
}