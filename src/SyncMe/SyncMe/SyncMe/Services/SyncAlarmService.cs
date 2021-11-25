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
            if (TryGetNearestAlarmDelay(syncEvent, out var alarmDelay))
            {
                syncALarm = new SyncAlarm(syncEvent.Title, eventId, (int)alarmDelay.TotalSeconds);
                return true;
            }
        }

        syncALarm = default;
        return false;
    }

    private bool TryGetNearestAlarmDelay(SyncEvent syncEvent, out TimeSpan delay)
    {
        var eventDateTime = syncEvent.Start - TimeSpan.FromSeconds((int)syncEvent.Alert.Reminder);
        TimeSpan eventTime = eventDateTime.TimeOfDay;

        delay = syncEvent.Schedule.Repeat switch
        {
            SyncRepeat.None => DelayAgainstNow(eventDateTime),
            SyncRepeat.Dayly => FirstAvailable(DateTime.Today.Add(eventTime), Dayly),
            SyncRepeat.WorkDays => FirstAvailable(DateTime.Today.Add(eventTime), WorkDays),
            SyncRepeat.EveryWeek => FirstAvailable(DateTime.Today.Add(eventTime), EveryWeek),
            SyncRepeat.EveryMonth => FirstAvailable(DateTime.Today.Add(eventTime), EveryMonth),
            SyncRepeat.EveryYear => FirstAvailable(DateTime.Today.Add(eventTime), EveryYear),
            SyncRepeat.EveryMinute => FirstAvailable(DateTime.Today.Add(eventTime), EveryMinute),
            _ => TimeSpan.Zero,
        };

        return delay.Ticks > 0;
    }

    private TimeSpan FirstAvailable(DateTime since, Func<DateTime, DateTime> next)
    {
        var current = since;

        while (true)
        {
            var delay = DelayAgainstNow(current);
            if (delay.Ticks > 100)
                return delay;

            current = next(current);
        }
    }

    private static DateTime Dayly(DateTime date) => date.AddDays(1);
    private static TimeSpan DelayAgainstNow(DateTime current) => current.Subtract(DateTime.Now);
    private static DateTime EveryWeek(DateTime date) => date.AddDays(7);
    private static DateTime EveryMonth(DateTime date) => date.AddMonths(1);
    private static DateTime EveryYear(DateTime date) => date.AddYears(1);
    private static DateTime EveryMinute(DateTime date) => date.AddMinutes(1);
    private static DateTime WorkDays(DateTime date) => date.DayOfWeek switch
    {
        DayOfWeek.Friday => date.AddDays(3),
        DayOfWeek.Saturday => date.AddDays(2),
        _ => date.AddDays(1)
    };
}
