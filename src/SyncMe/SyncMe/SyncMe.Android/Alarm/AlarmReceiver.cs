using Android.Content;
using SyncMe.Droid.Extensions;
using SyncMe.Models;

namespace SyncMe.Droid.Alarm;

[BroadcastReceiver]
internal class AlarmReceiver : BroadcastReceiver
{
    private readonly IAndroidAlarmService _androidAlarmService = Bootstrapper.GetService<IAndroidAlarmService>();

    public override void OnReceive(Context context, Intent intent)
    {
        var action = intent.GetStringExtra(AlarmMessage.ActionKey);

        switch (action)
        {
            case AlarmMessage.SetAlarmAction:
                SetAlarm(context, intent);
                return;

            case AlarmMessage.StopAlarmAction:
                StopAlarm(intent);
                return;

            default:
                return;
        }
    }

    private void SetAlarm(Context context, Intent intent)
    {
        var pendingAlarm = intent.GetExtra<SyncAlarm>();
        AndroidAlarmPlayer.Instance.PlayAlarm(context);
        AndroidNotificationManager.Instance.Show(pendingAlarm, context);
        _androidAlarmService.SetAlarm(pendingAlarm.EventId, context);
    }

    private void StopAlarm(Intent intent)
    {
        AndroidAlarmPlayer.Instance.StopPlaying();
        var id = intent.GetIntExtra(AlarmMessage.NotificationIdKey, -1);
        AndroidNotificationManager.Instance.Cancel(id);
    }
}
