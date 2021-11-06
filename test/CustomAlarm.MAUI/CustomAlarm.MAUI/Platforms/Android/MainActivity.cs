using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Icu.Text;
using Android.Icu.Util;
using Android.OS;
using Android.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Essentials;

namespace CustomAlarm.MAUI
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : MauiAppCompatActivity
	{
        private IDisposable _subscription;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);

            _subscription = Observable
                .FromEventPattern(h => MainPage.OnSetAlarmClickedEvent += h, h => MainPage.OnSetAlarmClickedEvent -= h)
                .Subscribe(x => SetAlarm(1));
        }

        private void SetAlarm(int number)
        {
            var simpleDateFormat = new SimpleDateFormat("HH:mm:ss");
            var am = GetSystemService(AlarmService) as AlarmManager;
            var calendar = Calendar.Instance;
            var calendarList = Enumerable.Range(1, number).Select(x => calendar).ToList();
            var textTimer = new StringBuilder("Alarm has been set").AppendLine();
            foreach (var calendarItem in calendarList)
            {
                calendarItem.Add(CalendarField.Second, 10);

                var requestCode = (int)calendar.TimeInMillis / 1000;
                var intent = new Intent(this, typeof(MyAlarmReceiver));
                intent.PutExtra("REQUEST_CODE", requestCode);
                intent.AddFlags(ActivityFlags.IncludeStoppedPackages);
                intent.AddFlags(ActivityFlags.ReceiverForeground);
                var pi = PendingIntent.GetBroadcast(this, requestCode, intent, 0);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                    am.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, calendarItem.TimeInMillis, pi);
                else
                    am.SetExact(AlarmType.RtcWakeup, calendarItem.TimeInMillis, pi);

                textTimer
                    .Append(simpleDateFormat.Format(calendarItem.TimeInMillis))
                    .AppendLine();
            }

            string value = textTimer.ToString();
            Console.WriteLine(value);
            Toast.MakeText(this, value, ToastLength.Short).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            _subscription?.Dispose();
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}