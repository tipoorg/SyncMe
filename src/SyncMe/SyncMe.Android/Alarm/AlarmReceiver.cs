using Android.Content;
using Android.Util;
using SyncMe.Droid.Extensions;
using SyncMe.Models;

namespace SyncMe.Droid.Alarm;

[BroadcastReceiver]
internal class AlarmReceiver : BroadcastReceiver
{
    private readonly IAlarmProcessor _androidAlarmProcessor = Bootstrapper.GetService<IAlarmProcessor>();

    public override void OnReceive(Context context, Intent intent)
    {
        try
        {
            var action = intent.GetStringExtra(MessageKeys.ActionKey);
            Log.Debug(MainActivity.Tag, $"AlarmReceiver.OnReceive {action}");

            switch (action)
            {
                case MessageKeys.ProcessAlarmAction:
                    var pendingAlarm = intent.GetExtra<SyncAlarm>();
                    _androidAlarmProcessor.ProcessAlarm(pendingAlarm);
                    return;

                case MessageKeys.StopAlarmAction:
                    var notificationId = intent.GetIntExtra(MessageKeys.NotificationIdKey, -1);
                    _androidAlarmProcessor.StopPlayingAlarm(notificationId);
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
