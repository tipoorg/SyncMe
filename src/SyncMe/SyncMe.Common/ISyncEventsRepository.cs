using System.Linq.Expressions;
using SyncMe.Models;
using SyncMe.Queries;

namespace SyncMe;

public interface ISyncEventsRepository
{
    bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent);
    SyncEvent AddSyncEvent(SyncEvent syncEvent);
    void RemoveEvents(Expression<Func<SyncEvent, bool>> predicate);
    void RemoveEvent(Guid eventId);
    IReadOnlyCollection<SyncEvent> GetByNamespace(string namespaceKey);
    void UpdateEvents(IEnumerable<SyncEvent> syncEvents);
    IReadOnlyCollection<SyncEvent> GetNotRepeatableEvents(SyncEventQuery query);
    IReadOnlyCollection<SyncEvent> GetEveryYearRepeatableEvents(SyncEventQuery query);
    IReadOnlyCollection<SyncEvent> GetRepeatableLessMonthEvents(SyncEventQuery query);
    IReadOnlyCollection<SyncEvent> GetEveryMonthRepeatableEvents(SyncEventQuery query);
}
