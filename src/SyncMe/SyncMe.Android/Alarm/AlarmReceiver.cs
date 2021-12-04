using Android.Content;
using Serilog;
using SyncMe.Droid.Extensions;
using SyncMe.Models;

namespace SyncMe.Droid.Alarm;

[BroadcastReceiver]
internal class AlarmReceiver : BroadcastReceiver
{
    private readonly IAlarmProcessor _androidAlarmProcessor = AndroidStarter.GetService<IAlarmProcessor>();

    public override void OnReceive(Context context, Intent intent)
    {
        try
        {
            var action = intent.GetStringExtra(MessageKeys.ActionKey);
            Log.Information($"AlarmReceiver.OnReceive {action}");

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
                    return;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }
}
