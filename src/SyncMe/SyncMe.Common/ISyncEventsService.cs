using System.Linq.Expressions;
using SyncMe.Models;

namespace SyncMe
{
    public interface ISyncEventsService
    {
        Guid AddSyncEvent(SyncEvent syncEvent);
        IReadOnlyCollection<SyncEvent> GetAllSyncEvents();
        void RemoveEvents(Expression<Func<SyncEvent, bool>> predicate);
        bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent);
        void TryRemoveInternalEvent(Guid eventId);
    }
}