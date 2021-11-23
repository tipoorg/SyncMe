using Android.Content;
using SyncMe.Droid.Extensions;
using SyncMe.Extensions;
using SyncMe.Models;
using SyncMe.Repos;

namespace SyncMe.Droid.Alarm;

[BroadcastReceiver]
internal class AlarmReceiver : BroadcastReceiver
{
    private ISyncEventsRepository _syncEventsRepository = Bootstrapper.GetService<ISyncEventsRepository>();

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
        var syncEvent = intent.GetExtra<SyncEvent>();
        if (_syncEventsRepository.TryGetSyncEvent(syncEvent.Id, out var storedEvent) 
            && storedEvent.Status == SyncStatus.Active)
        {
            return;
        }

        AndroidAlarmPlayer.Instance.PlayAlarm(context);
        AndroidNotificationManager.Instance.Show(syncEvent, context);
        new AndroidAlarmIntent().SetAlarm(syncEvent.DecrementRemainingTimes(), context);
    }

    private void StopAlarm(Intent intent)
    {
        AndroidAlarmPlayer.Instance.StopPlaying();
        var id = intent.GetIntExtra(AlarmMessage.NotificationIdKey, -1);
        AndroidNotificationManager.Instance.Cancel(id);
    }
}
