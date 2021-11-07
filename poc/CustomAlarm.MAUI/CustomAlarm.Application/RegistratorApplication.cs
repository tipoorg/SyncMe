using Microsoft.Extensions.DependencyInjection;

namespace CustomAlarm.Application;

public static class RegistratorApplication
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
}