using SyncMe.Models;

namespace SyncMe;

public interface ISyncNamespaceService
{
    IReadOnlyCollection<(string FullName, bool IsActive, bool HasChildren)> GetAll(string namespaceName);

    bool UpdateStatusWithChildrens(string FullName, bool IsActive, DateTime? turnOnDate = default);

    bool HasChildren(string fullName);
    void Add(string fullName);
    bool ParentIsSuspended(string fullname);

    IReadOnlyCollection<(string FullName, bool IsActive, bool HasChilde)> GetFirstLevel();
    IReadOnlyCollection<(string FullName, bool IsActive, bool HasChildren)> GetFirstChildren(string fullName);

    void RemoveWithChildren(string fullName);
    bool IsNamespaceActive(string fullName);
    IReadOnlyCollection<Namespace> GetAllSyncNamespaces();
}

