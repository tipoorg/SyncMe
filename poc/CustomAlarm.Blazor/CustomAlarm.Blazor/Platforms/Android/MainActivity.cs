using Android.App;
using Android.Content.PM;
using Microsoft.Maui;
using System;

namespace CustomAlarm.Blazor
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : MauiAppCompatActivity
	{
        private readonly IDisposable _setAlarmSubscription;

        public MainActivity()
        {
            var setAlarmClicks = App.GetRequiredService<MainPage>().SetAlarmClicks;
            _setAlarmSubscription = setAlarmClicks.Subscribe(x => SetAlarm());
        }

        private void SetAlarm()
        {
            Console.WriteLine("TEST");
        }
    }
}