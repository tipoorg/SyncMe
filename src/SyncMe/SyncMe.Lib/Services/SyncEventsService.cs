using System.Linq.Expressions;
using SyncMe.Models;
using SyncMe.Queries;

namespace SyncMe.Lib.Services;

internal sealed class SyncEventsService : ISyncEventsService
{
    private readonly ISyncEventsRepository _syncEventsRepository;
    private readonly IAlarmService _alarmService;
    private readonly ISyncAlarmCalculator _syncAlarmCalculator;

    public SyncEventsService(
        ISyncEventsRepository syncEventsRepository,
        IAlarmService alarmService,
        ISyncAlarmCalculator syncAlarmCalculator)
    {
        _syncEventsRepository = syncEventsRepository;
        _alarmService = alarmService;
        _syncAlarmCalculator = syncAlarmCalculator;
    }

    public bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent)
    {
        return _syncEventsRepository.TryGetSyncEvent(id, out syncEvent);
    }

    public IReadOnlyCollection<SyncEvent> SearchSyncEvents(SyncEventQuery syncEventQuery)
    {
        return _syncEventsRepository.SearchSyncEvents(syncEventQuery);
    }

    public Guid AddSyncEvent(SyncEvent syncEvent)
    {
        var newEvent = _syncEventsRepository.AddSyncEvent(syncEvent);

        if (_syncAlarmCalculator.TryGetNearestAlarm(newEvent, out var syncAlarm))
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
}
