using SyncMe.Models;

namespace SyncMe.Lib.Extensions;

public static class SyncEventExtensions
{
    public static bool TryGetNearestAlarm(this SyncEvent syncEvent, out SyncAlarm syncAlarm)
    {
        if (TryGetNearestAlarmTime(syncEvent, out var alarmDelay))
        {
            syncAlarm = new SyncAlarm(syncEvent.Title, syncEvent.Id, syncEvent.NamespaceKey, alarmDelay);
            return true;
        }

        syncAlarm = default;
        return false;
    }

    public static IEnumerable<DateTime> EnumerateEventTimes(this SyncEvent e, int month, int year) => e.Repeat switch
    {
        SyncRepeat.None => Enumerable.Repeat(e.Start, 1),
        SyncRepeat.Dayly => Generate(e.Start, Dayly),
        SyncRepeat.WorkDays => Generate(e.Start, WorkDays),
        SyncRepeat.EveryWeek => Generate(e.Start, EveryWeek),
        SyncRepeat.EveryMonth => Generate(new DateTime(year, month, e.Start.Day), EveryMonth),
        SyncRepeat.EveryYear => Generate(new DateTime(year, month, e.Start.Day), EveryYear),
        SyncRepeat.EveryMinute => Enumerable.Repeat(e.Start, 1),
        _ => throw new NotImplementedException(),
    };

    private static bool TryGetNearestAlarmTime(SyncEvent syncEvent, out DateTime alarmTime)
    {
        var now = DateTime.Now;
        var eventDateTime = syncEvent.Start - TimeSpan.FromMinutes((int)syncEvent.Reminder);
        TimeSpan eventTime = eventDateTime.TimeOfDay;

        alarmTime = syncEvent.Repeat switch
        {
            SyncRepeat.None => eventDateTime,
            SyncRepeat.Dayly => Generate(DateTime.Today.Add(eventTime), Dayly).First(x => x > now),
            SyncRepeat.WorkDays => Generate(DateTime.Today.Add(eventTime), WorkDays).First(x => x > now),
            SyncRepeat.EveryWeek => Generate(DateTime.Today.Add(eventTime), EveryWeek).First(x => x > now),
            SyncRepeat.EveryMonth => Generate(DateTime.Today.Add(eventTime), EveryMonth).First(x => x > now),
            SyncRepeat.EveryYear => Generate(DateTime.Today.Add(eventTime), EveryYear).First(x => x > now),
            SyncRepeat.EveryMinute => Generate(DateTime.Today.Add(eventTime), EveryMinute).First(x => x > now),
            _ => throw new NotImplementedException(),
        };

        return alarmTime > now;
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

    private static IEnumerable<T> Generate<T>(T seed, Func<T, T> next)
    {
        var cur = seed;

        while (true)
        {
            yield return cur;
            cur = next(cur);
        }
    }
}
