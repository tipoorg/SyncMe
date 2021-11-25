using SyncMe.Models;

namespace SyncMe.Repos;

public interface ISyncEventsRepository
{
    event EventHandler<Guid> OnAddSyncEvent;

    IReadOnlyCollection<SyncEvent> GetAllSyncEvents();
    bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent);
    Guid AddSyncEvent(SyncEvent syncEvent);
    void RemoveEvents(Func<SyncEvent, bool> predicate);
}
