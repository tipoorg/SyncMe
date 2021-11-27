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
    }

    public bool TryGetSyncEvent(Guid id, out SyncEvent syncEvent)
    {
        syncEvent = _events.FindById(id);
        return syncEvent != null;
    }

    public IReadOnlyCollection<SyncEvent> GetAllSyncEvents()
    {
        return _events.FindAll().ToList();
    }

    public Guid AddSyncEvent(SyncEvent syncEvent)
    {
        var newId = Guid.NewGuid();
        _events.Insert(newId, syncEvent);

        return newId;
    }

    public void RemoveEvents(Expression<Func<SyncEvent, bool>> predicate)
    {
        _events.DeleteMany(predicate);
    }
}
