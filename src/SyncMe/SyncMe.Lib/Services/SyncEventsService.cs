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
        IEnumerable<(SyncEvent Event, DateTime Time)> ExtractTimes(IEnumerable<SyncEvent> syncEvents) => syncEvents
           .SelectMany(e => EnumerateEventTimes(e, query.Month, query.Year).Select(x => (Event: e, Time: x)))
           .TakeWhile(x => LastDay(x.Time.Year, x.Time.Month) <= LastDay(query.Year, query.Month));

        var times = ExtractTimes(_syncEventsRepository.GetNotRepeatableEvents(query))
            .Concat(ExtractTimes(_syncEventsRepository.GetRepeatableLessMonthEvents(query)))
            .Concat(ExtractTimes(_syncEventsRepository.GetEveryMonthRepeatableEvents(query)))
            .Concat(ExtractTimes(_syncEventsRepository.GetEveryYearRepeatableEvents(query)))
            .OrderBy(x => x.Time)
            .ToList();

        return times;
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

    private static IEnumerable<DateTime> EnumerateEventTimes(SyncEvent e, int month, int year) => e.Repeat switch
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
        var eventDateTime = syncEvent.Start - TimeSpan.FromSeconds((int)syncEvent.Reminder);
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
