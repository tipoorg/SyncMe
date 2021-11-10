using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reactive;

namespace CustomAlarm.Blazor;

public static class RegistratorMaui
{
    public static IServiceCollection AddMaui(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MainPage>();
        serviceCollection.AddSingleton<IButtonController, ButtonController>();
        return serviceCollection;
    }
}
