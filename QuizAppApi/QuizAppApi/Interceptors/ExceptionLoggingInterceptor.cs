using Castle.DynamicProxy;

namespace QuizAppApi.Interceptors;

public class ExceptionLoggingInterceptor : IAsyncInterceptor
{
    private readonly Serilog.ILogger _logger;

    public ExceptionLoggingInterceptor(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    private void LogException(Exception exception)
    {
        _logger.Error(exception, "Exception in {MethodName}. Message: {Message}", exception.Source, exception.Message);
    }

    public void InterceptSynchronous(IInvocation invocation)
    {
        try
        {
            invocation.Proceed();
        }
        catch (Exception e)
        {
            LogException(e);
            throw;
        }
    }

    public void InterceptAsynchronous(IInvocation invocation)
    {
        invocation.ReturnValue = InternalInterceptAsynchronous<object>(invocation);
    }

    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
    }

    private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
    {
        try
        {
            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            var result = await task;
            return result;
        }
        catch (Exception e)
        {
            LogException(e);
            throw;
        }
    }
}