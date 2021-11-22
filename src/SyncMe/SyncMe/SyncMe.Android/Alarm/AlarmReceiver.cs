using Android.Content;
using SyncMe.Services;

namespace SyncMe.Droid.Alarm;

[BroadcastReceiver]
internal class AlarmReceiver : BroadcastReceiver
{
    private readonly IAlarmSetter<Context> _alarmSetter = App.GetRequiredService<IAlarmSetter<Context>>();
    private readonly IAlarmPlayer<Context> _alarmPlayer = App.GetRequiredService<IAlarmPlayer<Context>>();
    private readonly INotificationManager<Context> _notificationManager = App.GetRequiredService<INotificationManager<Context>>();

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
        var times = intent.GetIntExtra(AlarmMessage.TimesKey, -1);

        if (times > 0)
        {
            _alarmPlayer.PlayAlarm(context);
            _notificationManager.Show("Alarm", "WakeUp", context);
            _alarmSetter.SetAlarm(times - 1, context);
        }
    }

    private void StopAlarm(Intent intent)
    {
        _alarmPlayer.StopPlaying();

        var id = intent.GetIntExtra(AlarmMessage.NotificationIdKey, -1);
        _notificationManager.Cancel(id);
    }
}
