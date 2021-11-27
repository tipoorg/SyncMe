using System.Linq.Expressions;
using SyncMe.Models;

namespace SyncMe
{
    public interface ISyncEventsService
    {
        event EventHandler OnSyncEventsUpdate;

        int AddSyncEvent(SyncEvent syncEvent);
        IReadOnlyCollection<SyncEvent> GetAllSyncEvents();
        void RemoveEvents(Expression<Func<SyncEvent, bool>> predicate);
        bool TryGetSyncEvent(int id, out SyncEvent syncEvent);
    }
}