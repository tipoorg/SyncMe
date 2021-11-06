using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CastomAlarm.Xamarin.AndroidApp
{
    [BroadcastReceiver]
    public class MyAlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var requestCode = intent.GetIntExtra("REQUEST_CODE", -1);
            Toast.MakeText(context, $"Alarm fired with request code {requestCode}", ToastLength.Short).Show();
        }
    }
}