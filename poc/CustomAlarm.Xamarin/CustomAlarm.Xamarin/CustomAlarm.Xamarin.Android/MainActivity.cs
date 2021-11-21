using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Icu.Text;
using Android.Icu.Util;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using CustomAlarm.Xamarin.Views;
using Xamarin.Essentials;

namespace CustomAlarm.Xamarin.Droid
{
    [Activity(Label = "CustomAlarm.Xamarin", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private IDisposable _setAlarmSubscription;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var app = new App();
            _setAlarmSubscription = app.GetRequiredService<NotesPage>().SetAlarmClicks.Subscribe(x => SetAlarm());

            LoadApplication(app);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void SetAlarm()
        {
            var simpleDateFormat = new SimpleDateFormat("HH:mm:ss");
            var calendarItem = GetCalendarItem();
            var requestCode = (int)calendarItem.TimeInMillis / 1000;
            var alarmIntent = GetAlarmIntent(requestCode);

            SetAlarm(calendarItem, alarmIntent);

            var textTimer = new StringBuilder("Alarm has been set")
                .AppendLine()
                .Append(simpleDateFormat.Format(calendarItem.TimeInMillis))
                .AppendLine();

            Toast.MakeText(this, textTimer.ToString(), ToastLength.Short).Show();
        }

        private static Calendar GetCalendarItem()
        {
            var calendarItem = Calendar.Instance;
            calendarItem.Add(CalendarField.Second, 10);
            return calendarItem;
        }

        private void SetAlarm(Calendar calendarItem, PendingIntent alarmIntent)
        {
            var am = GetSystemService(AlarmService) as AlarmManager;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                am.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, calendarItem.TimeInMillis, alarmIntent);
            else
                am.SetExact(AlarmType.RtcWakeup, calendarItem.TimeInMillis, alarmIntent);

        }

        private PendingIntent GetAlarmIntent(int requestCode)
        {
            var intent = new Intent(this, typeof(MyAlarmReceiver));
            intent.PutExtra("REQUEST_CODE", requestCode);
            intent.AddFlags(ActivityFlags.IncludeStoppedPackages);
            intent.AddFlags(ActivityFlags.ReceiverForeground);
            return PendingIntent.GetBroadcast(this, requestCode, intent, 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _setAlarmSubscription.Dispose();

            base.Dispose(disposing);
        }
    }
}