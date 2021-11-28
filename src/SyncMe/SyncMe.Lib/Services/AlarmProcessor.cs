using SyncMe.Models;

namespace SyncMe.Lib.Services;

internal class AlarmProcessor : IAlarmProcessor
{
    private readonly IAlarmService _alarmService;
    private readonly IAlarmPlayer _alarmPlayer;
    private readonly INotificationManager _notificationManager;
    private readonly ISyncEventsService _syncEventsService;
    private readonly ISyncNamespaceService _syncNamespaceService;
    private readonly ISyncAlarmCalculator _syncAlarmCalculator;
    private readonly IConfigRepository _configRepository;

    public AlarmProcessor(
        IAlarmService alarmService,
        IAlarmPlayer alarmPlayer,
        INotificationManager notificationManager,
        ISyncEventsService syncEventsService,
        ISyncNamespaceService syncNamespaceService,
        ISyncAlarmCalculator syncAlarmCalculator,
        IConfigRepository configRepository)
    {
        _alarmService = alarmService;
        _alarmPlayer = alarmPlayer;
        _notificationManager = notificationManager;
        _syncEventsService = syncEventsService;
        _syncNamespaceService = syncNamespaceService;
        _syncAlarmCalculator = syncAlarmCalculator;
        _configRepository = configRepository;
    }

    public void ProcessAlarm(SyncAlarm pendingAlarm)
    {
        if (!_syncEventsService.TryGetSyncEvent(pendingAlarm.EventId, out var syncEvent))
        {
            return;
        }

        if (_syncNamespaceService.IsNamespaceActive(pendingAlarm.NamespaceFullName))
        {
            if (!_configRepository.Get(ConfigKey.IsMute))
                _alarmPlayer.PlayAlarm();

            _notificationManager.Show(pendingAlarm);
        }

        if (_syncAlarmCalculator.TryGetNearestAlarm(syncEvent, out var syncAlarm))
        {
            _alarmService.SetAlarm(syncAlarm);
        }
    }

    public void StopPlayingAlarm(int notificationId)
    {
        _alarmPlayer.StopPlaying();
        _notificationManager.Cancel(notificationId);
    }
}
