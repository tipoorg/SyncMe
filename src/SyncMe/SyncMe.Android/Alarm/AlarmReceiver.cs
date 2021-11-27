using Android.Content;
using Android.Util;

namespace SyncMe.Droid.Alarm;

[BroadcastReceiver]
internal class AlarmReceiver : BroadcastReceiver
{
    private readonly IAndroidAlarmService _androidAlarmService = Bootstrapper.GetService<IAndroidAlarmService>();

    public override void OnReceive(Context context, Intent intent)
    {
        try
        {
            var action = intent.GetStringExtra(AlarmMessage.ActionKey);
            Log.Debug(MainActivity.Tag, $"AlarmReceiver.OnReceive {action}");

            switch (action)
            {
                case AlarmMessage.ProcessAlarmAction:
                    _androidAlarmService.ProcessAlarm(context, intent);
                    return;

                case AlarmMessage.StopAlarmAction:
                    _androidAlarmService.StopPlayingAlarm(intent);
                    return;

                default:
                    Log.Debug(MainActivity.Tag, "AlarmReceiver.OnReceive default");
                    return;
            }
        }
        catch (Exception ex)
        {
            Log.Error(MainActivity.Tag, ex.Message);
        }
    }
}
