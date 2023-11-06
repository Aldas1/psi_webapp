using System;
using Castle.DynamicProxy;
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
            _logger.LogError(ex, "Exception in method {MethodName}", invocation.Method.Name);
        }
    }
}