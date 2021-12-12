using System.Linq.Expressions;
using SyncMe.Lib.Extensions;
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
           .SelectMany(e => e.EnumerateEventTimes(query.Month, query.Year).Select(x => (Event: e, Time: x)))
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

        _alarmService.SetAlarmForEvent(syncEvent);

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

    private static DateTime LastDay(int year, int month) => new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
}
