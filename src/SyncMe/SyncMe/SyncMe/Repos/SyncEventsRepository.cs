using SyncMe.Models;

namespace SyncMe.Repos;

internal sealed class SyncEventsRepository : ISyncEventsRepository
{
    private readonly Dictionary<Guid, SyncEvent> _events = new();

    public SyncEventsRepository()
    {
        var e1 = new SyncEvent(Guid.NewGuid(), "First", "", default, new SyncSchedule(SyncRepeat.Every10Seconds, 2), default, SyncStatus.Stopped);
        var e2 = new SyncEvent(Guid.NewGuid(), "Second", "", default, new SyncSchedule(SyncRepeat.Every10Seconds, 4), default, SyncStatus.Stopped);

        _events.Add(e1.Id, e1);
        _events.Add(e2.Id, e2);
    }

    public bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent) => _events.TryGetValue(id, out syncEvent);

    public IReadOnlyCollection<SyncEvent> GetAllSyncEvents()
    {
        var res = _events.Values.ToList();
        return res;
    }
}
