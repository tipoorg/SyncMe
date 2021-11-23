using SyncMe.Models;

namespace SyncMe.Repos;

public interface ISyncEventsRepository
{
    IReadOnlyCollection<SyncEvent> GetAllSyncEvents();
    bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent);
    void AddSyncEvent(SyncEvent syncEvent);
}
