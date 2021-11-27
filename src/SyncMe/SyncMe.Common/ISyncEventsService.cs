using SyncMe.Models;

namespace SyncMe
{
    public interface ISyncEventsService
    {
        event EventHandler OnSyncEventsUpdate;

        Guid AddSyncEvent(SyncEvent syncEvent);
        IReadOnlyCollection<SyncEvent> GetAllSyncEvents();
        void RemoveEvents(Func<SyncEvent, bool> predicate);
        bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent);
    }
}