using System;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.Icu.Util;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Xamarin.Essentials;

namespace CastomAlarm.Xamarin.AndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btn_set_alarm;
        TextView txt_timer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Init();
        }

        private void Init()
        {
            btn_set_alarm = FindViewById<Button>(Resource.Id.btn_timer);
            txt_timer = FindViewById<TextView>(Resource.Id.txt_timer);

            btn_set_alarm.Click += delegate
            {
                SetAlarm(5);
            };
        }

        private void SetAlarm(int number)
        {
            var simpleDateFormat = new SimpleDateFormat("HH:mm:ss");
            var am = GetSystemService(AlarmService) as AlarmManager;
            var calendar = Calendar.Instance;
            var calendarList = Enumerable.Range(1, number).Select(x => calendar).ToList();
            var textTimer = new StringBuilder();
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

            txt_timer.Text = textTimer.ToString();
            Toast.MakeText(this, "Alarm has been set", ToastLength.Short).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}