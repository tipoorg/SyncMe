﻿using System.Linq.Expressions;
using LiteDB;
using SyncMe.Models;
using SyncMe.Queries;

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

    public IReadOnlyCollection<SyncEvent> SearchSyncEvents(SyncEventQuery query)
    {
        var queryable = _events.Query();
        queryable = query.StartMonth is int month ? queryable.Where(x => x.Start.Month == month) : queryable;
        queryable = query.StartYear is int year ? queryable.Where(x => x.Start.Year == year) : queryable;
        
        return queryable
            .OrderBy(x => x.Start)
            .ToArray();
    }

    public SyncEvent AddSyncEvent(SyncEvent syncEvent)
    {
        var newId = Guid.NewGuid();
        _events.Insert(newId, syncEvent);

        return _events.FindById(newId);
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
