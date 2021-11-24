using SyncMe.Models;

namespace SyncMe.Repos;

public interface ISyncNamespaceRepository
{
    Dictionary<string, Namespace> GetAllSyncNamespaces();
    void AddSyncNamespace(string name);
    bool TryGetSyncNamespace(string name, out Namespace existingNamespace);
}
