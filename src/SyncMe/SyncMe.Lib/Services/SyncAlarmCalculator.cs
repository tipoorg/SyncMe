using SyncMe.Models;

namespace SyncMe.Lib.Services;

internal class SyncAlarmCalculator : ISyncAlarmCalculator
{
    public bool TryGetNearestAlarm(SyncEvent syncEvent, out SyncAlarm syncALarm)
    {
        if (TryGetNearestAlarmDelay(syncEvent, out var alarmDelay))
        {
            syncALarm = new SyncAlarm(syncEvent.Title, syncEvent.Id, syncEvent.NamespaceKey, alarmDelay);
            return true;
        }

        syncALarm = default;
        return false;
    }

    private bool TryGetNearestAlarmDelay(SyncEvent syncEvent, out DateTime alarmTime)
    {
        var eventDateTime = syncEvent.Start - TimeSpan.FromSeconds((int)syncEvent.Reminder);
        TimeSpan eventTime = eventDateTime.TimeOfDay;

        alarmTime = syncEvent.Repeat switch
        {
            SyncRepeat.None => eventDateTime,
            SyncRepeat.Dayly => FirstAvailable(DateTime.Today.Add(eventTime), Dayly),
            SyncRepeat.WorkDays => FirstAvailable(DateTime.Today.Add(eventTime), WorkDays),
            SyncRepeat.EveryWeek => FirstAvailable(DateTime.Today.Add(eventTime), EveryWeek),
            SyncRepeat.EveryMonth => FirstAvailable(DateTime.Today.Add(eventTime), EveryMonth),
            SyncRepeat.EveryYear => FirstAvailable(DateTime.Today.Add(eventTime), EveryYear),
            SyncRepeat.EveryMinute => FirstAvailable(DateTime.Today.Add(eventTime), EveryMinute),
            _ => throw new NotImplementedException(),
        };

        return alarmTime > DateTime.Now;
    }

    private DateTime FirstAvailable(DateTime since, Func<DateTime, DateTime> next)
    {
        var now = DateTime.Now;
        var current = since;

        while (true)
        {
            if (current > now)
                return current;

            current = next(current);
        }
    }

    private static DateTime Dayly(DateTime date) => date.AddDays(1);
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
