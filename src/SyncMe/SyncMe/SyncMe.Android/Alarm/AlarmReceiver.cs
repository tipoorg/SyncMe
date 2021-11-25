using Android.Content;
using Android.Widget;
using SyncMe.Droid.Extensions;
using SyncMe.Models;

namespace SyncMe.Droid.Alarm;

[BroadcastReceiver]
internal class AlarmReceiver : BroadcastReceiver
{
    private readonly IAndroidAlarmService _androidAlarmService = Bootstrapper.GetService<IAndroidAlarmService>();
    private readonly IAndroidAlarmPlayer _androidAlarmPlayer = Bootstrapper.GetService<IAndroidAlarmPlayer>();

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
                Toast.MakeText(context, "OnReceive is unhandled", ToastLength.Long).Show();
                return;
        }
    }

    private void SetAlarm(Context context, Intent intent)
    {
        var pendingAlarm = intent.GetExtra<SyncAlarm>();
        _androidAlarmPlayer.PlayAlarm(context);
        AndroidNotificationManager.Instance.Show(pendingAlarm, context);
        _androidAlarmService.SetAlarm(pendingAlarm.EventId, context);
    }

    private void StopAlarm(Intent intent)
    {
        _androidAlarmPlayer.StopPlaying();
        var id = intent.GetIntExtra(AlarmMessage.NotificationIdKey, -1);
        AndroidNotificationManager.Instance.Cancel(id);
    }
}
