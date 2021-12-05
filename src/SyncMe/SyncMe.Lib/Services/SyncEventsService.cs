using System.Linq.Expressions;
using SyncMe.Models;
using SyncMe.Queries;

namespace SyncMe.Lib.Services;

internal sealed class SyncEventsService : ISyncEventsService
{
    private readonly ISyncEventsRepository _syncEventsRepository;
    private readonly IAlarmService _alarmService;

    public SyncEventsService(
        ISyncEventsRepository syncEventsRepository,
        IAlarmService alarmService)
    {
        _syncEventsRepository = syncEventsRepository;
        _alarmService = alarmService;
    }

    public bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent)
    {
        return _syncEventsRepository.TryGetSyncEvent(id, out syncEvent);
    }

    public IReadOnlyCollection<(SyncEvent Event, DateTime Time)> SearchSyncEventTimes(SyncEventQuery query)
    {
        var events = _syncEventsRepository.GetNotRepeatableEvents(query)
            .Concat(_syncEventsRepository.GetRepeatableLessMonthEvents(query))
            .Concat(_syncEventsRepository.GetEveryMonthRepeatableEvents(query))
            .Concat(_syncEventsRepository.GetEveryYearRepeatableEvents(query))
            .SelectMany(e => GetEventTimes(e, query.Month, query.Year).Select(x => (e, x)))
            .ToList();

        return events;
    }

    public Guid AddSyncEvent(SyncEvent syncEvent)
    {
        var newEvent = _syncEventsRepository.AddSyncEvent(syncEvent);

        if (TryGetNearestAlarm(newEvent, out var syncAlarm))
        {
            _alarmService.SetAlarm(syncAlarm);
        }

        return newEvent.Id;
    }

    public void RemoveEvents(Expression<Func<SyncEvent, bool>> predicate)
    {
        _syncEventsRepository.RemoveEvents(predicate);
    }

    public void TryRemoveInternalEvent(Guid eventId)
    {
        if (_syncEventsRepository.TryGetSyncEvent(eventId, out var syncEvent) && syncEvent.ExternalId is null)
        {
            _syncEventsRepository.RemoveEvent(eventId);
        }
    }

    public bool TryGetNearestAlarm(SyncEvent syncEvent, out SyncAlarm syncAlarm)
    {
        if (TryGetNearestAlarmTime(syncEvent, out var alarmDelay))
        {
            syncAlarm = new SyncAlarm(syncEvent.Title, syncEvent.Id, syncEvent.NamespaceKey, alarmDelay);
            return true;
        }

        syncAlarm = default;
        return false;
    }

    private static IEnumerable<DateTime> GetEventTimes(SyncEvent e, int month, int year) => e.Repeat switch
    {
        SyncRepeat.None => Enumerable.Repeat(e.Start, 1),
        SyncRepeat.Dayly => Enumerate(e.Start, Dayly, Until(month, year)),
        SyncRepeat.WorkDays => Enumerate(e.Start, WorkDays, Until(month, year)),
        SyncRepeat.EveryWeek => Enumerate(e.Start, EveryWeek, Until(month, year)),
        SyncRepeat.EveryMonth => Enumerate(new(year, month, e.Start.Day), EveryMonth, Until(month, year)),
        SyncRepeat.EveryYear => Enumerate(new(year, month, e.Start.Day), EveryYear, Until(month, year)),
        SyncRepeat.EveryMinute => Enumerable.Repeat(e.Start, 1),
        _ => throw new NotImplementedException(),
    };

    private static bool TryGetNearestAlarmTime(SyncEvent syncEvent, out DateTime alarmTime)
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

    private static DateTime LastDay(int year, int month) => new DateTime(year, month, 1).AddMonths(1).AddDays(-1);

    private static Func<DateTime, bool> Until(int month, int year) => x => LastDay(x.Year, x.Month) <= LastDay(year, month);

    private static IEnumerable<DateTime> Enumerate(DateTime seed, Func<DateTime, DateTime> next, Func<DateTime, bool> until)
    {
        var cur = seed;
        while (until(cur))
        {
            yield return cur;
            cur = next(cur);
        }
    }

    private static DateTime FirstAvailable(DateTime since, Func<DateTime, DateTime> next)
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
}
