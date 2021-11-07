using Android.Content;
using Android.Media;
using Android.Widget;

namespace CustomAlarm.MAUI;

[BroadcastReceiver]
internal class MyAlarmReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        var requestCode = intent.GetIntExtra("REQUEST_CODE", -1);
        PlayRingtone(context);
        Toast.MakeText(context, $"Alarm fired with request code {requestCode}", ToastLength.Short).Show();
    }

    private static void PlayRingtone(Context context)
    {
        var soundUri = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);
        var ringtone = RingtoneManager.GetRingtone(context, soundUri);
        ringtone.Play();
    }
}
