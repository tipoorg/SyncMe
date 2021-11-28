using LiteDB;
using SyncMe.Models;

namespace SyncMe.DataAccess.Repos;

internal class SyncNamespaceRepository : ISyncNamespaceRepository
{
    private ILiteCollection<Namespace> _namespaces;

    public SyncNamespaceRepository(ILiteDatabase database)
    {
        _namespaces = database.GetCollection<Namespace>();
        BsonMapper.Global.Entity<Namespace>().Id(x => x.Key);
        SeedData();
    }

    private void SeedData()
    {
        if (_namespaces.Count() is 0)
        {
            _namespaces.Insert(new Namespace[]
            {
                new() { Key = "Work", IsActive = true },
                new() { Key = "Work.Team1", IsActive = true },
                new() { Key = "Work.Team2", IsActive = true },
                new() { Key = "Outlook", IsActive = true },
            });
        }
    }

    public Dictionary<string, Namespace> GetAllSyncNamespaces()
    {
        var result = _namespaces.FindAll().ToDictionary(k => k.Key);
        return result;
    }

    public void TryAddSyncNamespace(string namespaceKey, bool isActive = true)
    {
        if (_namespaces.FindById(namespaceKey) is null)
        {
            _namespaces.Insert(new Namespace { Key = namespaceKey, IsActive = isActive });
        }
    }

    public bool TryGetSyncNamespace(string namespaceKey, out Namespace existingNamespace)
    {
        existingNamespace = _namespaces.FindById(namespaceKey);
        return existingNamespace != null;
    }

    public void RemoveNamespace(string namespaceKey)
    {
        _namespaces.Delete(namespaceKey);
    }
}
