using System.Linq.Expressions;
using SyncMe.Models;

namespace SyncMe.Lib.Services;

internal sealed class SyncEventsService : ISyncEventsService
{
    public event EventHandler OnSyncEventsUpdate;

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

    public bool TryGetSyncEvent(int id, out SyncEvent syncEvent) => _syncEventsRepository.TryGetSyncEvent(id, out syncEvent);

    public IReadOnlyCollection<SyncEvent> GetAllSyncEvents() => _syncEventsRepository.GetAllSyncEvents();

    public int AddSyncEvent(SyncEvent syncEvent)
    {
        var newId = _syncEventsRepository.AddSyncEvent(syncEvent);

        if (_syncAlarmCalculator.TryGetNearestAlarm(newId, out var syncAlarm))
        {
            _alarmService.SetAlarm(syncAlarm);
        }

        OnSyncEventsUpdate?.Invoke(this, default);
        return newId;
    }

    public void RemoveEvents(Expression<Func<SyncEvent, bool>> predicate)
    {
        _syncEventsRepository.RemoveEvents(predicate);
        OnSyncEventsUpdate?.Invoke(this, default);
    }
}
