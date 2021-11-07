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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;

namespace CustomAlarm.MAUI
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : MauiAppCompatActivity
	{
        private readonly IGeneralEventsController _generalEventsController;
        private readonly IDisposable _onSetAlarmSubscription;

        public MainActivity()
        {
            _generalEventsController = App.ServiceProvider.GetService<IGeneralEventsController>();
            _onSetAlarmSubscription = Observable
              .FromEventPattern(_generalEventsController, nameof(IGeneralEventsController.OnSetAlarmClickedEvent))
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
            Toast.MakeText(this, value, ToastLength.Short).Show();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _onSetAlarmSubscription.Dispose();

            base.Dispose(disposing);
        }
    }
}