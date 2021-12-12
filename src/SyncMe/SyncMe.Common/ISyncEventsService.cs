using System.Linq.Expressions;
using SyncMe.Models;
using SyncMe.Queries;

namespace SyncMe
{
    public interface ISyncEventsService
    {
        Guid AddSyncEvent(SyncEvent syncEvent);
        void RemoveEvents(Expression<Func<SyncEvent, bool>> predicate);
        bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent);
        void TryRemoveInternalEvent(Guid eventId);
        IReadOnlyCollection<(SyncEvent Event, DateTime Time)> SearchSyncEventTimes(SyncEventQuery syncEventQuery);
    }
}