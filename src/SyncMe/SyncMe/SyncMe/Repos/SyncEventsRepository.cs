using SyncMe.Models;

namespace SyncMe.Repos;

internal sealed class SyncEventsRepository : ISyncEventsRepository
{
    private Dictionary<Guid, SyncEvent> _events = new();

    public bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent) => _events.TryGetValue(id, out syncEvent);

    public IReadOnlyCollection<SyncEvent> GetAllSyncEvents()
    {
        var res = _events.Values.ToList();
        return res;
    }

    public Guid AddSyncEvent(SyncEvent syncEvent)
    {
        var newId = Guid.NewGuid();
        _events.Add(newId, syncEvent);
        return newId;
    }
    public void RemoveEvents(Func<SyncEvent, bool> predicate)
    {
        _events = _events.Where(p => !predicate(p.Value)).ToDictionary(p => p.Key, p => p.Value);
    }
}
