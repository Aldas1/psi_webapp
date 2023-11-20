using Castle.DynamicProxy;

namespace QuizAppApi.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddProxiedScoped<TInterface, TImplementation>(this IServiceCollection serviceCollection)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        serviceCollection.AddScoped<TImplementation>();
        serviceCollection.AddScoped(typeof(TInterface), ImplementationFactory<TInterface, TImplementation>);
        return serviceCollection;
    }
    
    public static IServiceCollection AddProxiedTransient<TInterface, TImplementation>(this IServiceCollection serviceCollection)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        serviceCollection.AddTransient<TImplementation>();
        serviceCollection.AddTransient(typeof(TInterface), ImplementationFactory<TInterface, TImplementation>);
        return serviceCollection;
    }
    
    public static IServiceCollection AddProxiedSingleton<TInterface, TImplementation>(this IServiceCollection serviceCollection)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        serviceCollection.AddSingleton<TImplementation>();
        serviceCollection.AddSingleton(typeof(TInterface), ImplementationFactory<TInterface, TImplementation>);
        return serviceCollection;
    }

    private static object ImplementationFactory<TInterface, TImplementation>(IServiceProvider provider)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
        var impl = provider.GetRequiredService<TImplementation>();
        var interceptors = provider.GetServices<IAsyncInterceptor>().ToArray();
        return proxyGenerator.CreateInterfaceProxyWithTarget(typeof(TInterface), impl, interceptors);
    }
}