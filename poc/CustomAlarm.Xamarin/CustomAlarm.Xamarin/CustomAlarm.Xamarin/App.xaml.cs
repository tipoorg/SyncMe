using System;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace CustomAlarm.Xamarin
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public T GetRequiredService<T>() => _serviceProvider.GetRequiredService<T>();

        public App()
        {
            _serviceProvider = BuildServiceProvider();

            InitializeComponent();

            MainPage = GetRequiredService<AppShell>();
        }

        private IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection()
              .AddPages();

            return DIDataTemplate.AppServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
