using System;
using CustomAlarm.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using MaUIApplication = Microsoft.Maui.Controls.Application;

namespace CustomAlarm.MAUI;

public partial class App : MaUIApplication
{
    private static readonly IServiceProvider _serviceProvider = CreateServices();
 
    public static T GetRequiredService<T>() => _serviceProvider.GetRequiredService<T>();

    public App()
    {
        InitializeComponent();

        MainPage = _serviceProvider.GetService<MainPage>();
    }


    private static IServiceProvider CreateServices()
    {
        var serviceCollection = new ServiceCollection()
            .AddMaui()
            .AddApplication();

        return serviceCollection.BuildServiceProvider();
    }
}
