using Microsoft.Extensions.DependencyInjection;

namespace CustomAlarm.MAUI;

public static class RegistratorMaui
{
    public static IServiceCollection AddMaui(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MainPage>();
        return serviceCollection;
    }
}