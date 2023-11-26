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

    public async void InterceptAsynchronous(IInvocation invocation)
    {
        await InternalInterceptAsynchronous(invocation);
    }

    public async void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        await InternalInterceptAsynchronous(invocation);
    }

    private async Task InternalInterceptAsynchronous(IInvocation invocation)
    {
        try
        {
            invocation.Proceed();
            var task = (Task)invocation.ReturnValue;
            await task;
        }
        catch (Exception e)
        {
            LogException(e);
            throw;
        }
    }
}