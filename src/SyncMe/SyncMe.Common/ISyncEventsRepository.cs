using System.Linq.Expressions;
using SyncMe.Models;

namespace SyncMe;

public interface ISyncEventsRepository
{
    IReadOnlyCollection<SyncEvent> GetAllSyncEvents();
    bool TryGetSyncEvent(int id, out SyncEvent syncEvent);
    int AddSyncEvent(SyncEvent syncEvent);
    void RemoveEvents(Expression<Func<SyncEvent, bool>> predicate);
}
