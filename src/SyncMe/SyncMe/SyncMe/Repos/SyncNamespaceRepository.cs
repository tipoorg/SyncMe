using System.Threading;
using SyncMe.Models;

namespace SyncMe.Repos;

public class SyncNamespaceRepository : ISyncNamespaceRepository
{
    private static int _idCounter = 0;
    private readonly Dictionary<string, IReadOnlyCollection<Namespace>> _existingNamespaces = new();

    public SyncNamespaceRepository()
    {
        SeedData();
    }

    private void SeedData()
    {
        _existingNamespaces.Add("Test", new List<Namespace> { CreateNamespace("Test") });
        _existingNamespaces.Add("Another.Test", new List<Namespace> { CreateNamespace("Another"), CreateNamespace("Test") });
    }

    private void Increment(ref int counter) => Interlocked.Increment(ref counter);

    private Namespace CreateNamespace(string title)
    {
        Increment(ref _idCounter);
        return new Namespace(_idCounter, title);
    }

    public Dictionary<string, IReadOnlyCollection<Namespace>> GetAllSyncNamespaces() => 
        _existingNamespaces;

    private IReadOnlyCollection<Namespace> GetAllSyncNamespaces(Dictionary<int, Namespace> existingNamespaces, List<Namespace> namespaces)
    {
        foreach (var space in existingNamespaces)
        {
            namespaces.Add(new Namespace(space.Key, space.Value.Title));
        }

        return namespaces;
    }

    public void AddSyncNamespace(string name)
    {
        _existingNamespaces.Add(name, GenerateSubNamespaces(name.Split('.')));
    }

    private IReadOnlyCollection<Namespace> GenerateSubNamespaces(params string[] subspaces)
    {
        var namespaces = new List<Namespace>();
        foreach (var subspace in subspaces)
        {
            Increment(ref _idCounter);
            var newNamespace = new Namespace(_idCounter, subspace);
            namespaces.Add(newNamespace);
        }

        return namespaces;
    }

    public bool TryGetSyncNamespace(string name, out IReadOnlyCollection<Namespace> existingNamespaces) =>
        _existingNamespaces.TryGetValue(name, out existingNamespaces);
}
