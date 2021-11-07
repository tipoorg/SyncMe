using System;
using CustomAlarm.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using MaUIApplication = Microsoft.Maui.Controls.Application;

namespace CustomAlarm.MAUI;

public partial class App : MaUIApplication
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public App()
    {
        ServiceProvider = CreateServices();

        InitializeComponent();


        MainPage = ServiceProvider.GetService<MainPage>();
    }


    private IServiceProvider CreateServices()
    {
        var serviceCollection = new ServiceCollection()
            .AddMaui()
            .AddApplication();

        return serviceCollection.BuildServiceProvider();
    }
}
