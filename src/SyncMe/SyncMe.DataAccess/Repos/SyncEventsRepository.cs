using System.Linq.Expressions;
using SyncMe.Models;

namespace SyncMe.DataAccess.Repos;

internal sealed class SyncEventsRepository : ISyncEventsRepository
{
    private readonly ApplicationContext _context;

    public SyncEventsRepository(ApplicationContext context)
    {
        _context = context;
    }

    public bool TryGetSyncEvent(int id, out SyncEvent syncEvent)
    {
        syncEvent =_context.Events.Find(id);
        return syncEvent != null;
    }

    public IReadOnlyCollection<SyncEvent> GetAllSyncEvents()
    {
        return _context.Events.ToList();
    }

    public int AddSyncEvent(SyncEvent syncEvent)
    {
        var res = _context.Events.Add(syncEvent);
        _context.SaveChanges();

        return res.Entity.Id;
    }

    public void RemoveEvents(Expression<Func<SyncEvent, bool>> predicate)
    {
        var toRemove = _context.Events.Where(predicate).ToList();
        _context.Events.RemoveRange(toRemove);
    }
}
