using SyncMe.Models;

namespace SyncMe.Repos;

public interface ISyncNamespaceRepository
{
    IReadOnlyCollection<Namespace> GetAllSyncNamespacrs();
    bool TryAddSyncNamespace(string name, out int namespaceId);
    bool TryGetSyncNamespace(string name, out Namespace existingNamespace);
    bool TryGetSyncNamespace(int id, out Namespace existingNamespace);
}
