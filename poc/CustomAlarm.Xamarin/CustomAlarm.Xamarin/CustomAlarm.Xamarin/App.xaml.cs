using System;
using CustomAlarm.Xamarin.Services;
using CustomAlarm.Xamarin.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomAlarm.Xamarin
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
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
