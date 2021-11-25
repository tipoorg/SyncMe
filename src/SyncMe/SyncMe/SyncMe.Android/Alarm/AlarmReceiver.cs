using Android.Content;
using Android.Widget;

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
            case AlarmMessage.ProcessAlarmAction:
                _androidAlarmService.ProcessAlarm(context, intent);
                return;

            case AlarmMessage.StopAlarmAction:
                _androidAlarmService.StopPlayingAlarm(intent);
                return;

            default:
                Toast.MakeText(context, "OnReceive is unhandled", ToastLength.Long).Show();
                return;
        }
    }
}
