using System.Linq.Expressions;
using LiteDB;
using SyncMe.Models;

namespace SyncMe.DataAccess.Repos;

internal sealed class SyncEventsRepository : ISyncEventsRepository
{
    private readonly ILiteCollection<SyncEvent> _events;

    public SyncEventsRepository(ILiteDatabase database)
    {
        _events = database.GetCollection<SyncEvent>();
        BsonMapper.Global.Entity<SyncEvent>().Id(x => x.Id);
    }

    public bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent)
    {
        syncEvent = _events.FindById(id);
        return syncEvent != null;
    }

    public IReadOnlyCollection<SyncEvent> GetByNamespace(string namespaceKey)
    {
        _events.EnsureIndex(x => x.NamespaceKey);
        var result = _events.Find(x => x.NamespaceKey == namespaceKey).ToList();
        return result;
    }

    public IReadOnlyCollection<SyncEvent> GetAllSyncEvents()
    {
        var result = _events.FindAll().ToList();
        return result;
    }

    public Guid AddSyncEvent(SyncEvent syncEvent)
    {
        var newId = Guid.NewGuid();
        _events.Insert(newId, syncEvent);

        return newId;
    }

    public void UpdateEvents(IEnumerable<SyncEvent> syncEvents)
    {
        _events.Update(syncEvents);
    }

    public void RemoveEvent(Guid eventId)
    {
        _events.Delete(eventId);
    }

    public void RemoveEvents(Expression<Func<SyncEvent, bool>> predicate)
    {
        _events.DeleteMany(predicate);
    }
}
