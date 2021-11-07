using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Application = Microsoft.Maui.Controls.Application;

namespace CustomAlarm.MAUI
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider;

        public App()
        {
            ServiceProvider = CreateServices();

            InitializeComponent();


            MainPage = ServiceProvider.GetService<MainPage>();
        }


        private IServiceProvider CreateServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<MainPage>();
            serviceCollection.AddSingleton<IGeneralEventsController, GeneralEventsController>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
