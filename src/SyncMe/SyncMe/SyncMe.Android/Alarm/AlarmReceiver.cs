using Android.Content;
using Android.Media;

namespace SyncMe.Droid.Alarm;

[BroadcastReceiver]
internal class AlarmReceiver : BroadcastReceiver
{
    private readonly IAlarmSetter _alarmSetter = App.GetRequiredService<IAlarmSetter>();

    public override void OnReceive(Context context, Intent intent)
    {
        var times = intent.GetIntExtra("TIMES", -1);

        if (times > 0)
        {
            PlayAlarm(context);
            _alarmSetter.SetAlarm(times - 1, context);
        }
    }

    private static void PlayAlarm(Context context)
    {
        var mp = new MediaPlayer();
        var soundUri = RingtoneManager.GetActualDefaultRingtoneUri(context, RingtoneType.Notification);

        try
        {
            mp.Reset();
            mp.SetDataSource(context, soundUri);
            mp.SetAudioAttributes(GetAudio());
            mp.Prepare();
            mp.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static AudioAttributes GetAudio()
    {
        return new AudioAttributes.Builder()
            .SetUsage(AudioUsageKind.Alarm)
            .Build();
    }
}
