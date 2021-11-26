using System.Collections.Concurrent;
using SyncMe.Models;

namespace SyncMe.Repos;

internal sealed class SyncEventsRepository : ISyncEventsRepository
{
    public event EventHandler<Guid> OnAddSyncEvent;
    public event EventHandler OnSyncEventsUpdate;

    private ConcurrentDictionary<Guid, SyncEvent> _events = new();

    public bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent) => _events.TryGetValue(id, out syncEvent);

    public IReadOnlyCollection<SyncEvent> GetAllSyncEvents()
    {
        var res = _events.Values.ToList();
        return res;
    }

    public Guid AddSyncEvent(SyncEvent syncEvent)
    {
        var newId = Guid.NewGuid();
        _events.AddOrUpdate(newId, syncEvent, (_,__) => syncEvent);
        OnAddSyncEvent?.Invoke(this, newId);
        OnSyncEventsUpdate?.Invoke(this, default);
        return newId;
    }

    public void RemoveEvents(Func<SyncEvent, bool> predicate)
    {
        _events = new(_events.Where(p => !predicate(p.Value)).ToDictionary(p => p.Key, p => p.Value));
        OnSyncEventsUpdate?.Invoke(this, default);
    }
}
