using System.Threading;
using SyncMe.Models;

namespace SyncMe.Repos;

public class SyncNamespaceRepository : ISyncNamespaceRepository
{
    private static int _idCounter = 0;
    private readonly Dictionary<string, Namespace> _existingNamespaces = new();

    public SyncNamespaceRepository()
    {
        SeedData();
    }

    private void SeedData()
    {
        _existingNamespaces.Add("Test", CreateNamespace("Test"));
        _existingNamespaces.Add("Another.Test", CreateNamespace("Another"));
    }

    private void Increment(ref int counter) => Interlocked.Increment(ref counter);

    private Namespace CreateNamespace(string title)
    {
        Increment(ref _idCounter);
        return new Namespace() { Title = title };
    }

    public Dictionary<string, Namespace> GetAllSyncNamespaces() => 
        _existingNamespaces;

    public void AddSyncNamespace(string name)
    {
        _existingNamespaces.Add(name, new Namespace { Title = name });
    }

    public bool TryGetSyncNamespace(string name, out Namespace existingNamespace) =>
        _existingNamespaces.TryGetValue(name, out existingNamespace);
}
