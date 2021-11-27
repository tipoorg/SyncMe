using SyncMe.Models;

namespace SyncMe;

public interface ISyncEventsRepository
{
    event EventHandler<Guid> OnAddSyncEvent;
    event EventHandler OnSyncEventsUpdate;

    IReadOnlyCollection<SyncEvent> GetAllSyncEvents();
    bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent);
    Guid AddSyncEvent(SyncEvent syncEvent);
    void RemoveEvents(Func<SyncEvent, bool> predicate);
}
