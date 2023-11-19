using QuizAppApi.Events;

namespace QuizAppApi.Extensions;

public static class ProgramEventSetupExtensions
{
    public static IServiceCollection AddEventHandler<TEventHandler>(this IServiceCollection serviceCollection)
        where TEventHandler : EventHandlerBase
    {
        serviceCollection.AddSingleton<EventHandlerBase, TEventHandler>();
        return serviceCollection;
    }

    public static IApplicationBuilder UseEventHandlers(this IApplicationBuilder applicationBuilder)
    {
        foreach (var service in applicationBuilder.ApplicationServices.GetServices<EventHandlerBase>())
        {
            service.RegisterEventHandler();
        };
        return applicationBuilder;
    }
}