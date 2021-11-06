using Android.Content;
using Android.Widget;

namespace CustomAlarm.MAUI
{
    [BroadcastReceiver]
    internal class MyAlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var requestCode = intent.GetIntExtra("REQUEST_CODE", -1);
            Toast.MakeText(context, $"Alarm fired with request code {requestCode}", ToastLength.Short).Show();
        }
    }
}
