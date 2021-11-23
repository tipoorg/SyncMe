using SyncMe.Models;

namespace SyncMe.Repos;

public interface ISyncNamespaceRepository
{
    Dictionary<string, IReadOnlyCollection<Namespace>> GetAllSyncNamespaces();
    void AddSyncNamespace(string name);
    bool TryGetSyncNamespace(string name, out IReadOnlyCollection<Namespace> existingNamespaces);
}
