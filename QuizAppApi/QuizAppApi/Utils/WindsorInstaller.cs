using Castle.MicroKernel.Registration;
using Castle.Windsor;
using QuizAppApi.Interfaces;
using QuizAppApi.Repositories;
using QuizAppApi.Services;

namespace QuizAppApi.Utils;

public class WindsorInstaller : IWindsorInstaller
{
    public void Install(IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
    {
        container.Register(
            Component.For<IQuizRepository>().ImplementedBy<QuizRepository>()
                .Interceptors<ExceptionLoggingInterceptor>()
                .LifestyleTransient(),

            Component.For<IUserRepository>().ImplementedBy<UserRepository>()
                .Interceptors<ExceptionLoggingInterceptor>()
                .LifestyleTransient(),

            Component.For<IExplanationService>().ImplementedBy<ExplanationService>()
                .Interceptors<ExceptionLoggingInterceptor>()
                .LifestyleTransient(),
            Component.For<IQuizService>().ImplementedBy<QuizService>()
                .Interceptors<ExceptionLoggingInterceptor>()
                .LifestyleTransient()
        );
    }
}