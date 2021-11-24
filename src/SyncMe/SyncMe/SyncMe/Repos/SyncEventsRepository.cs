using SyncMe.Models;

namespace SyncMe.Repos;

internal sealed class SyncEventsRepository : ISyncEventsRepository
{
    private readonly Dictionary<Guid, SyncEvent> _events = new();

    public bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent) => _events.TryGetValue(id, out syncEvent);

    public IReadOnlyCollection<SyncEvent> GetAllSyncEvents()
    {
        var res = _events.Values.ToList();
        return res;
    }

    public Guid AddSyncEvent(SyncEvent syncEvent)
    {
        var guid = Guid.NewGuid();
        _events.Add(guid, syncEvent);
        return guid;
    }
}
