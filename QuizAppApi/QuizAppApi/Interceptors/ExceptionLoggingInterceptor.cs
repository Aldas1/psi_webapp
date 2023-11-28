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
        invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
    }

    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
    }

    private async Task InternalInterceptAsynchronous(IInvocation invocation)
    {
        try
        {
            invocation.Proceed();
            await (Task) invocation.ReturnValue;
        }
        catch (Exception e)
        {
            LogException(e);
            throw;
        }
    }
    
    private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
    {
        try
        {
            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            return await task;
        }
        catch (Exception e)
        {
            LogException(e);
            throw;
        }
    }
}