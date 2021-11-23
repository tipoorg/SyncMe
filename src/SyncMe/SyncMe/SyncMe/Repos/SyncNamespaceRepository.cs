using System.Threading;
using SyncMe.Models;

namespace SyncMe.Repos;

public class SyncNamespaceRepository : ISyncNamespaceRepository
{
    private readonly object syncLock = new();
    private static int _idCounter = 0;
    private readonly Dictionary<int, Namespace> _existingNamespaces = new Dictionary<int, Namespace>();

    public SyncNamespaceRepository()
    {
        SeedData();
    }

    private void SeedData()
    {
        Increment(ref _idCounter);
        _existingNamespaces.Add(_idCounter, new Namespace(_idCounter, "Test"));

        Increment(ref _idCounter);
        _existingNamespaces.Add(_idCounter, new Namespace(_idCounter, "Another"));
    }

    private void Increment(ref int counter) => Interlocked.Increment(ref counter);

    public IReadOnlyCollection<Namespace> GetAllSyncNamespacrs()
    {
        return GetAllSyncNamespaces(_existingNamespaces, new List<Namespace>());
    }

    private IReadOnlyCollection<Namespace> GetAllSyncNamespaces(Dictionary<int, Namespace> existingNamespaces, List<Namespace> namespaces)
    {
        foreach (var space in existingNamespaces)
        {
            namespaces.Add(new Namespace(space.Key, space.Value.Title));
        }

        return namespaces;
    }

    public bool TryAddSyncNamespace(string name, out int namespaceId)
    {
        namespaceId = default;
        lock (syncLock)
        {
            Increment(ref _idCounter);
            try
            {
                _existingNamespaces.Add(_idCounter, new Namespace(_idCounter, name));
                namespaceId = _idCounter;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public bool TryGetSyncNamespace(string name, out Namespace existingNamespace) =>
        GetAllSyncNamespacrs().ToDictionary(x => x.Title, y => y).TryGetValue(name, out existingNamespace);

    public bool TryGetSyncNamespace(int id, out Namespace existingNamespace) => _existingNamespaces.TryGetValue(id, out existingNamespace);
}
