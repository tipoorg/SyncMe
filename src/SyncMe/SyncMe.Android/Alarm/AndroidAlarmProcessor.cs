using Android.Content;
using SyncMe.Droid.Extensions;
using SyncMe.Models;

namespace SyncMe.Droid.Alarm;

internal class AndroidAlarmProcessor : IAndroidAlarmProcessor
{
    private readonly IAlarmService _alarmService;
    private readonly IAndroidAlarmPlayer _androidAlarmPlayer;
    private readonly ISyncNamespaceService _syncNamespaceService;
    private readonly ISyncAlarmCalculator _syncAlarmCalculator;

    public AndroidAlarmProcessor(
        IAlarmService alarmService,
        IAndroidAlarmPlayer androidAlarmPlayer,
        ISyncNamespaceService syncNamespaceService,
        ISyncAlarmCalculator syncAlarmCalculator)
    {
        _alarmService = alarmService;
        _androidAlarmPlayer = androidAlarmPlayer;
        _syncNamespaceService = syncNamespaceService;
        _syncAlarmCalculator=syncAlarmCalculator;
    }

    public void ProcessAlarm(Context context, Intent intent)
    {
        var pendingAlarm = intent.GetExtra<SyncAlarm>();
        if (_syncNamespaceService.IsNamespaceActive(pendingAlarm.NamespaceFullName))
        {
            _androidAlarmPlayer.PlayAlarm(context);
            AndroidNotificationManager.Instance.Show(pendingAlarm, context);
        }

        if (_syncAlarmCalculator.TryGetNearestAlarm(pendingAlarm.EventId, out var syncAlarm))
        {
            _alarmService.SetAlarm(syncAlarm);
        }
    }

    public void StopPlayingAlarm(Intent intent)
    {
        _androidAlarmPlayer.StopPlaying();
        var id = intent.GetIntExtra(MessageKeys.NotificationIdKey, -1);
        AndroidNotificationManager.Instance.Cancel(id);
    }
}
