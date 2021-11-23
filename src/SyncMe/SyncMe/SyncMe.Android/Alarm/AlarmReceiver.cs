using Android.Content;

namespace SyncMe.Droid.Alarm;

[BroadcastReceiver]
internal class AlarmReceiver : BroadcastReceiver
{
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
            AndroidAlarmPlayer.Instance.PlayAlarm(context);
            AndroidNotificationManager.Instance.Show("Alarm", context);
            new AndroidAlarmIntent().SetAlarm(times - 1, context);
        }
    }

    private void StopAlarm(Intent intent)
    {
        AndroidAlarmPlayer.Instance.StopPlaying();
        var id = intent.GetIntExtra(AlarmMessage.NotificationIdKey, -1);
        AndroidNotificationManager.Instance.Cancel(id);
    }
}
