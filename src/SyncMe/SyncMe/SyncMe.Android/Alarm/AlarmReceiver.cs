using Android.Content;
using Android.Media;
using Android.Widget;

namespace SyncMe.Droid.Alarm;

[BroadcastReceiver]
internal class AlarmReceiver : BroadcastReceiver
{
    private readonly IAlarmSetter _alarmSetter = App.GetRequiredService<IAlarmSetter>();

    public override void OnReceive(Context context, Intent intent)
    {
        PlayRingtone(context);

        var times = intent.GetIntExtra("TIMES", -1);
        
        if (times > 0)
        {
            _alarmSetter.SetAlarm(times - 1, context);
        }
        else
        {
            Toast.MakeText(context, $"It was Last Alarm", ToastLength.Short).Show();
        }
    }

    private static void PlayRingtone(Context context)
    {
        var soundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
        var ringtone = RingtoneManager.GetRingtone(context, soundUri);
        ringtone.Play();
    }
}
