using Microsoft.Extensions.DependencyInjection;

namespace CustomAlarm.Application;

public static class RegistratorApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IGeneralEventsController, GeneralEventsController>();
        return serviceCollection;
    }
}