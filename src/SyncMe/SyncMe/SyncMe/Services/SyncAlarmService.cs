using SyncMe.Models;
using SyncMe.Repos;

namespace SyncMe.Services;

public class SyncAlarmService : ISyncAlarmService
{
    private readonly ISyncEventsRepository _syncEventsRepository;

    public SyncAlarmService(ISyncEventsRepository syncEventsRepository)
    {
        _syncEventsRepository = syncEventsRepository;
    }

    public bool TryGetNearestAlarm(Guid eventId, out SyncAlarm syncALarm)
    {
        if (_syncEventsRepository.TryGetSyncEvent(eventId, out var syncEvent))
        {
            if (TryGetNearestAlarmDelay(syncEvent.Start, syncEvent.Alert.Reminders, out var alarmDelay))
            {
                syncALarm = new SyncAlarm(syncEvent.Title, eventId, (int)alarmDelay.TotalSeconds);
                return true;
            }
        }

        syncALarm = default;
        return false;
    }

    private bool TryGetNearestAlarmDelay(DateTime eventStartTime, SyncReminder[] reminders, out TimeSpan delay)
    {
        var earliestReminder = reminders.Cast<int>().Max();
        var eventDateTime = eventStartTime - TimeSpan.FromSeconds(earliestReminder);

        delay = eventDateTime - DateTime.Now;
        return delay.Ticks > 0;
    }
}
